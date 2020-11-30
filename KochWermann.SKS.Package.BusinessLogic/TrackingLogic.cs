using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using KochWermann.SKS.Package.BusinessLogic.Validators;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using KochWermann.SKS.Package.ServiceAgents.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using KochWermann.SKS.Package.BusinessLogic.Helpers;
using FluentValidation;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace KochWermann.SKS.Package.BusinessLogic
{
    public class TrackingLogic : ITrackingLogic
    {
        private readonly IMapper _mapper;
        private readonly IParcelRepository _parcelRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IWebhookRepository _webhookRepository;
        private readonly ILogger _logger;
        private readonly IGeoEncodingAgent _geoEncodingAgent;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ParcelValidator _parcelValidator = new ParcelValidator();
        private readonly TrackingIdValidator _trackingIdValidator = new TrackingIdValidator();
        private readonly CodeValidator _codeValidator = new CodeValidator();
        private readonly Random _random = new Random();
        private readonly IWebhookAgent _webhook;

        public TrackingLogic(IMapper mapper,
                             IParcelRepository parcelRepository,
                             IWebhookRepository webhookRepository,
                             ILogger<TrackingLogic> logger,
                             IGeoEncodingAgent geoEncodingAgent,
                             IWarehouseRepository warehouseRepository,
                             IHttpClientFactory clientFactory,
                             IWebhookAgent webhook)
        {
            _parcelRepository = parcelRepository;
            _warehouseRepository = warehouseRepository;
            _webhookRepository = webhookRepository;
            _mapper = mapper;
            _logger = logger;
            _geoEncodingAgent = geoEncodingAgent;
            _clientFactory = clientFactory;
            _webhook = webhook;
            _logger.LogTrace("TrackingLogic created");
        }

        public Parcel TransitionParcel(Parcel parcel, string trackingId)
        {
            try
            {
                BusinessLogicHelper.Validate<Parcel>(parcel, _parcelValidator, _logger);
                BusinessLogicHelper.Validate<string>(trackingId, _trackingIdValidator, _logger);

                if (_parcelRepository.ContainsTrackingId(trackingId))
                {
                    throw new BLException($"TrackingId: {trackingId} is already used");
                }

                parcel.TrackingId = trackingId;
                SubmitParcel(parcel);

                return parcel;
            }
            catch (ValidationException ex)
            {
                _logger.LogError($"Validation error in parcel or trackingId {trackingId} {ex}");
                throw new BLValidationException($"Validation error in parcel or trackingId {trackingId}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in TransitionParcel {ex}");
                throw new BLException("Error in TransitionParcel", ex);
            }
        }

        public Parcel TrackParcel(string trackingId)
        {
            try
            {
                BusinessLogicHelper.Validate<string>(trackingId, _trackingIdValidator, _logger);

                var dalParcel = _parcelRepository.GetParcelByTrackingId(trackingId);
                var blParcel = _mapper.Map<BusinessLogic.Entities.Parcel>(dalParcel);

                return blParcel;
            }
            catch (DataAccess.Entities.DALNotFoundException ex)
            {
                _logger.LogError($"Could not find parcel with trackingId {trackingId} {ex}");
                throw new BLNotFoundException($"Could not find parcel with trackingId {trackingId}", ex);
            }
            catch (ValidationException ex)
            {
                _logger.LogError($"Validation error in trackingId {trackingId} {ex}");
                throw new BLValidationException($"Validation error in trackingId {trackingId}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in TrackParcel {ex}");
                throw new BLException("Error in TrackParcel", ex);
            }
        }

        public Parcel SubmitParcel(Parcel parcel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(parcel.TrackingId))
                {
                    parcel.TrackingId = CreateNewUniqueTrackingID();
                }

                BusinessLogicHelper.Validate<Parcel>(parcel, _parcelValidator, _logger);

                parcel.FutureHops = PredictFutureHops(parcel);
                parcel.State = Parcel.StateEnum.PickupEnum;

                _parcelRepository.Create(_mapper.Map<DataAccess.Entities.Parcel>(parcel));

                return parcel;
            }
            catch (DataAccess.Entities.DALNotFoundException ex)
            {
                _logger.LogError($"Could not find any truck or transfer warehouse near the sender's or the receiver's location {ex}");
                throw new BLNotFoundException($"Could not find any truck or transfer warehouse near the sender's or the receiver's location", ex);
            }
            catch (ValidationException ex)
            {
                _logger.LogError($"Validation error in parcel {ex}");
                throw new BLValidationException("Validation error in parcel", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in SubmitParcel {ex}");
                throw new BLException("Error in SubmitParcel", ex);
            }
        }

        public void ReportParcelDelivery(string trackingId)
        {
            try
            {
                BusinessLogicHelper.Validate<string>(trackingId, _trackingIdValidator, _logger);

                var parcel = _parcelRepository.GetParcelByTrackingId(trackingId);

                if (parcel.FutureHops?.Count > 0) _logger.LogWarning("hops to come: ");
                foreach (var futureHop in parcel.FutureHops)
                {
                    _logger.LogWarning($"\t{futureHop.Code} at {futureHop.DateTime.Value.ToShortTimeString()}");
                    futureHop.DateTime = DateTime.Now;
                    parcel.VisitedHops.Add(futureHop);
                }

                parcel.FutureHops.Clear();
                parcel.State = DataAccess.Entities.Parcel.StateEnum.DeliveredEnum;

                _parcelRepository.Update(parcel);

                var hooks = _webhookRepository.GetByTrackingId(trackingId);
                _webhook.Notify(_mapper.Map<IEnumerable<WebhookResponse>>(hooks), _mapper.Map<WebhookMessage>(parcel));
                _webhookRepository.Delete(hooks); 
            }
            catch (DataAccess.Entities.DALNotFoundException ex)
            {
                _logger.LogError($"Could not find parcel with trackingId {trackingId} {ex}");
                throw new BLNotFoundException($"Could not find parcel with trackingId {trackingId}", ex);
            }
            catch (ValidationException ex)
            {
                _logger.LogError($"Validation error in trackingId {trackingId} {ex}");
                throw new BLValidationException($"Validation error in trackingId {trackingId}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ReportParcelDelivery {ex}");
                throw new BLException("Error in ReportParcelDelivery", ex);
            }
        }

        public void ReportParcelHop(string trackingId, string code)
        {
            try
            {
                BusinessLogicHelper.Validate<string>(trackingId, _trackingIdValidator, _logger);
                BusinessLogicHelper.Validate<string>(code, _codeValidator, _logger);

                var parcel = _parcelRepository.GetParcelByTrackingId(trackingId);
                var hop = _warehouseRepository.GetHopByCode(code);

                if (!parcel.FutureHops.Exists(p => p.Code == code))
                {
                    _logger.LogWarning("Reported hop code is not part of the future hops of this parcel");
                    parcel.VisitedHops.Add(new DataAccess.Entities.HopArrival{
                        Code = hop.Code,
                        DateTime = DateTime.Now,
                        Description = hop.Description
                    });
                }
                else
                {
                    var visitedHops = new List<DataAccess.Entities.HopArrival>();

                    foreach (var futureHop in parcel.FutureHops)
                    {
                        visitedHops.Add(futureHop);
                        futureHop.DateTime = DateTime.Now;
                        parcel.VisitedHops.Add(futureHop);
                        if (futureHop.Code != hop.Code)
                            _logger.LogWarning($"skip hop {futureHop.Code} : {futureHop.DateTime.Value.ToShortTimeString()}");
                        else
                            break;
                    }

                    foreach (var visitedHop in visitedHops)
                    {
                        parcel.FutureHops.Remove(visitedHop);
                    }
                }

                if (hop.HopType == "TransferWarehouse")
                {
                    var transferWarehouse = (DataAccess.Entities.TransferWarehouse)hop;

                    var request = new HttpRequestMessage(HttpMethod.Post, $"{transferWarehouse.LogisticsPartnerUrl}/parcel/{parcel.TrackingId}");
                    var serilizedParcel = JsonConvert.SerializeObject(parcel, Formatting.None, new JsonSerializerSettings { 
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                    request.Content = new StringContent(serilizedParcel, Encoding.UTF8, "application/json");

                    var client = _clientFactory.CreateClient("parcelhop");
                    var response = client.SendAsync(request).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new BLException($"Transition of parcel {parcel.Id} to TransferWarehouse/LogisticPartner {transferWarehouse.LogisticsPartner} failed");
                    }

                    parcel.State = DataAccess.Entities.Parcel.StateEnum.TransferredEnum;
                }
                else if (hop.HopType == "Truck")
                {
                    parcel.State = DataAccess.Entities.Parcel.StateEnum.InTruckDeliveryEnum;
                }
                else if (hop.HopType == "Warehouse")
                {
                    parcel.State = DataAccess.Entities.Parcel.StateEnum.InTransportEnum;
                }

                _parcelRepository.Update(parcel);
            }
            catch (DataAccess.Entities.DALNotFoundException ex)
            {
                _logger.LogError($"Could not find parcel with trackingId {trackingId} or hop with code {code} {ex}");
                throw new BLNotFoundException($"Could not find parcel with trackingId {trackingId} or hop with code {code}", ex);
            }
            catch (ValidationException ex)
            {
                _logger.LogError($"Validation error in trackingId {trackingId} or code {code} {ex}");
                throw new BLValidationException($"Validation error in trackingId {trackingId} or code {code}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ReportParcelHop {ex}");
                throw new BLException("Error in ReportParcelHop", ex);
            }
        }

        private string CreateNewUniqueTrackingID(int attempts = 1)
        {
            if (attempts > 15)
            {
                _logger.LogError("Creating new unique trackingID failed");
                throw new BLException("Creating new unique trackingID failed");
            }

            var validCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var trackingId = new string(Enumerable.Repeat(validCharacters, 9).Select(s => s[_random.Next(s.Length)]).ToArray());

            if (_parcelRepository.ContainsTrackingId(trackingId))
            {
                trackingId = CreateNewUniqueTrackingID(++attempts);
            }
            return trackingId;
        }

        private static List<DataAccess.Entities.Hop> GetNextHops(Dictionary<string, DataAccess.Entities.Warehouse> hops, string root, string target)
        {
            var list = new List<DataAccess.Entities.Hop>();
            var rootHop = hops[root];

            foreach (var next in rootHop.NextHops)
            {
                if (next.Hop.HopType != "Warehouse")
                    continue;

                if (next.Hop.Code == target)
                {
                    list.Add(hops[target]);
                    return list;
                }

                list.AddRange(GetNextHops(hops, next.Hop.Code, target));
                if (list.Count <= 0)
                    continue;

                list.Add(hops[next.Hop.Code]);
                return list;
            }
            return list;
        }

        private List<HopArrival> PredictFutureHops(Parcel parcel)
        {
            var senderLocation = _geoEncodingAgent.AddressEncoder($"{parcel.Sender.Street}, {parcel.Sender.PostalCode} {parcel.Sender.City}, {parcel.Sender.Country}");
            var receiverLocation = _geoEncodingAgent.AddressEncoder($"{parcel.Recipient.Street}, {parcel.Recipient.PostalCode} {parcel.Recipient.City}, {parcel.Recipient.Country}");

            DataAccess.Entities.Hop senderTruckOrTransferWarehouse = _warehouseRepository.GetHopByCoordinates((double)senderLocation.Lon, (double)senderLocation.Lat);
            DataAccess.Entities.Hop receiverTruckOrTransferWarehouse = _warehouseRepository.GetHopByCoordinates((double)receiverLocation.Lon, (double)receiverLocation.Lat);

            var hopDictionary = new Dictionary<string, DataAccess.Entities.Warehouse>();
            var allWarehouses = _warehouseRepository.GetAllWarehouses();
            var rootWarehouse = _warehouseRepository.GetRootWarehouse();

            foreach (var warehouse in allWarehouses)
            {
                hopDictionary.Add(warehouse.Code, warehouse);
            }

            var senderWarehouse = allWarehouses.Single(h => h.NextHops.Any(nh => nh.Hop.Code == senderTruckOrTransferWarehouse.Code));
            var receiverWarehouse = allWarehouses.Single(h => h.NextHops.Any(nh => nh.Hop.Code == receiverTruckOrTransferWarehouse.Code));

            parcel.FutureHops = new List<HopArrival>();
            DateTime dateTime = DateTime.Now.AddMinutes((double)senderTruckOrTransferWarehouse.ProcessingDelayMins);
            parcel.FutureHops.Add(new HopArrival()
            {
                Code = senderTruckOrTransferWarehouse.Code,
                DateTime = dateTime,
                Description = senderTruckOrTransferWarehouse.Description
            });

            if (senderWarehouse.Code == receiverWarehouse.Code)
            {
                dateTime = DateTime.Now.AddMinutes((double)senderWarehouse.ProcessingDelayMins);
                parcel.FutureHops.Add(new HopArrival()
                {
                    Code = senderWarehouse.Code,
                    DateTime = dateTime,
                    Description = senderWarehouse.Description
                });
            }
            else
            {
                var hopsSender = GetNextHops(hopDictionary, rootWarehouse.Code, senderWarehouse.Code);
                hopsSender.Reverse();
                hopsSender.ForEach(delegate (DataAccess.Entities.Hop hop)
                {
                    _logger.LogInformation($"-> [{hop.Code}]");
                });

                var hopsReceiver = GetNextHops(hopDictionary, rootWarehouse.Code, receiverWarehouse.Code);
                hopsReceiver.ForEach(delegate (DataAccess.Entities.Hop hop)
                {
                    _logger.LogInformation($"-> [{hop.Code}]");
                });

                var intersection = hopsSender.Intersect(hopsReceiver).FirstOrDefault() ?? rootWarehouse;

                var route = hopsSender.TakeWhile(h => h != intersection).ToList();
                route.Add(intersection);
                route.AddRange(hopsReceiver.AsEnumerable().Reverse().TakeWhile(h => h != intersection));
                route.ToList().ForEach(delegate (DataAccess.Entities.Hop hop)
                {
                    _logger.LogInformation($"-> [{hop.Code}]");
                });

                foreach (var hop in route)
                {
                    dateTime = dateTime.AddMinutes((double)hop.ProcessingDelayMins);
                    parcel.FutureHops.Add(new HopArrival()
                    {
                        Code = hop.Code,
                        DateTime = dateTime,
                        Description = hop.Description
                    });
                }
            }

            dateTime = dateTime.AddMinutes((double)receiverTruckOrTransferWarehouse.ProcessingDelayMins);
            parcel.FutureHops.Add(new HopArrival()
            {
                Code = receiverTruckOrTransferWarehouse.Code,
                DateTime = dateTime,
                Description = receiverTruckOrTransferWarehouse.Description
            });

            return parcel.FutureHops;
        }

    }
}