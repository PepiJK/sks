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

namespace KochWermann.SKS.Package.BusinessLogic
{
    public class TrackingLogic : ITrackingLogic
    {
        private readonly IMapper _mapper;
        private readonly IParcelRepository _parcelRepository;
        private readonly IWarehouseRepository _warehouseRepository;

        private readonly ILogger _logger;
        private readonly ParcelValidator _parcelValidator = new ParcelValidator();
        private readonly TrackingIdValidator _trackingIdValidator = new TrackingIdValidator();
        private readonly CodeValidator _codeValidator = new CodeValidator();
        private const string validCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random _random = new Random();
        private readonly IGeoEncodingAgent _geoEncodingAgent;

        public TrackingLogic(IMapper mapper, IParcelRepository parcelRepository, ILogger<TrackingLogic> logger, IGeoEncodingAgent geoEncodingAgent, IWarehouseRepository warehouseRepository)
        {
            _parcelRepository = parcelRepository;
            _warehouseRepository = warehouseRepository;
            _mapper = mapper;
            _logger = logger;
            _logger.LogTrace("TrackingLogic created");
            _geoEncodingAgent = geoEncodingAgent;
        }

        private string CreateNewUniqueTrackingID(int attempts = 1)
        {
            if (attempts > 15)
            {
                _logger.LogError("Creating new unique trackingID failed!");
                throw new OutOfMemoryException("Creating new unique trackingID failed!");
            }

            var trackingId = new string(Enumerable.Repeat(validCharacters, 9).Select(s => s[_random.Next(s.Length)]).ToArray());
            if (_parcelRepository.ContainsTrackingID(trackingId))
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

            var senderTruck = _warehouseRepository.GetHopByCoordinates((double)senderLocation.Lon, (double)senderLocation.Lat) as DataAccess.Entities.Truck;
            var receiverTruck = _warehouseRepository.GetHopByCoordinates((double)receiverLocation.Lon, (double)receiverLocation.Lat) as DataAccess.Entities.Truck;

            // Developer mode:
            if (senderTruck == null || receiverTruck == null)
            {
                var allTrucks = _warehouseRepository.GetAllTrucks();
                senderTruck ??= allTrucks.FirstOrDefault();
                receiverTruck ??= allTrucks.LastOrDefault();
            }

            // maybe for release mode (depends on data given)
            // if (senderTruck == null || receiverTruck == null)
            // {
            //     _logger.LogError("This area has no truck");
            //     throw new Exception("This area has no truck");
            // }

            var hopDictionary = new Dictionary<string, DataAccess.Entities.Warehouse>();
            var allWarehouses = _warehouseRepository.GetAllWarehouses();
            var rootWarehouse = allWarehouses.FirstOrDefault();
            foreach (var warehouse in allWarehouses)
            {
                hopDictionary.Add(warehouse.Code, warehouse);
            }

            var senderWarehouse = allWarehouses.Single(h => h.NextHops.Any(nh => nh.Hop.Code == senderTruck.Code));
            var receiverWarehouse = allWarehouses.Single(h => h.NextHops.Any(nh => nh.Hop.Code == receiverTruck.Code));

            parcel.FutureHops = new List<HopArrival>();
            DateTime dateTime = DateTime.Now.AddMinutes((double)senderTruck.ProcessingDelayMins);
            parcel.FutureHops.Add(new HopArrival()
            {
                Code = senderTruck.Code,
                DateTime = dateTime
            });

            if (senderWarehouse.Code != receiverWarehouse.Code)
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
                        DateTime = dateTime
                    });
                }
            }
            dateTime = dateTime.AddMinutes((double)receiverTruck.ProcessingDelayMins);
            parcel.FutureHops.Add(new HopArrival()
            {
                Code = receiverTruck.Code,
                DateTime = dateTime
            });

            return parcel.FutureHops;
        }

        public Parcel TransitionParcel(Parcel parcel, string trackingId)
        {
            try
            {
                BusinessLogicHelper.Validate<Parcel>(parcel, _parcelValidator, _logger);
                BusinessLogicHelper.Validate<string>(trackingId, _trackingIdValidator, _logger);

                if (_parcelRepository.ContainsTrackingID(trackingId))
                {
                    throw new Exception($"TrackingId: {trackingId} is used allready.");
                }

                parcel.TrackingId = trackingId;
                SubmitParcel(parcel);
                return parcel;
            }
            catch (DataAccess.Entities.DALException ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (ValidationException ex)
            {
                throw BusinessLogicHelper.ValidationExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (Exception ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
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
                throw BusinessLogicHelper.NotFoundExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (DataAccess.Entities.DALException ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (ValidationException ex)
            {
                throw BusinessLogicHelper.ValidationExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (Exception ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
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
            catch (DataAccess.Entities.DALException ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (ValidationException ex)
            {
                throw BusinessLogicHelper.ValidationExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (Exception ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
        }

        public void ReportParcelDelivery(string trackingId)
        {
            try
            {
                BusinessLogicHelper.Validate<string>(trackingId, _trackingIdValidator, _logger);

                var parcel = _parcelRepository.GetParcelByTrackingId(trackingId);
                _logger.LogWarning("hops to come: ");
                foreach (var futureHop in parcel.FutureHops)
                {
                    _logger.LogWarning($"\t{futureHop.Code} at {futureHop.DateTime.Value.ToShortTimeString()}");
                    futureHop.DateTime = DateTime.Now;
                    parcel.VisitedHops.Add(futureHop);
                }
                parcel.FutureHops.Clear();
                parcel.State = DataAccess.Entities.Parcel.StateEnum.DeliveredEnum;
                _parcelRepository.Update(parcel);
            }
            catch (DataAccess.Entities.DALNotFoundException ex)
            {
                throw BusinessLogicHelper.NotFoundExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (DataAccess.Entities.DALException ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (ValidationException ex)
            {
                throw BusinessLogicHelper.ValidationExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (Exception ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
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

                if (hop.HopType == "TransferWarehouse")
                {
                    var transferWarehouse = (DataAccess.Entities.TransferWarehouse)hop;
                    using (var client = new HttpClient())
                    {
                        var parcelJson = Newtonsoft.Json.JsonConvert.SerializeObject(parcel);
                        var response = client.PostAsync(transferWarehouse.LogisticsPartnerUrl + "/parcel", new StringContent(parcelJson, System.Text.Encoding.UTF8, "application/json"));
                        if (response.IsCompletedSuccessfully == false)
                            throw new Exception($"Transition of parcel {parcel.Id} to TransferWarehouse/LogisticPartner {transferWarehouse.LogisticsPartner} failed: {response.Exception}");
                    }
                    parcel.State = DataAccess.Entities.Parcel.StateEnum.DeliveredEnum;
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
            /*
            catch (DataAccess.Entities.DAL_NotFound_Exception ex)
            {
                throw BusinessLogicHelper.NotFound_ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (DataAccess.Entities.DAL_Exception ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            */
            catch (ValidationException ex)
            {
                throw BusinessLogicHelper.ValidationExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (Exception ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
        }


    }
}