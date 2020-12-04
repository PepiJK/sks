using System;
using System.Collections.Generic;
using AutoMapper;
using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using NUnit.Framework;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using Moq;
using KochWermann.SKS.Package.BusinessLogic.Mapper;
using Microsoft.Extensions.Logging;

using BLException = KochWermann.SKS.Package.BusinessLogic.Entities.BLException;
using KochWermann.SKS.Package.ServiceAgents.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;
using Moq.Protected;
using System.Threading;
using System.Net;

namespace KochWermann.SKS.Package.BusinessLogic.Tests
{
    public class TrackingLogicTests
    {
        private ITrackingLogic _trackingLogic;
        private Parcel _validParcel;
        private string _validTrackingId = "PYJRB4HZ6";
        private string _notFoundTrackingId = "PYJRB4HZ9";
        private string _dupliicateTrackingId = "PYJRB4HZ7";
        private string _invalidTrackingId = "hallo";
        private string _validWarehouseCode = "WARE1234";
        private string _validTruckCode = "TRUC1234";
        private string _validTransferWarehouseCode = "TRAN1234";
        private string _invalidCode = "hi";
        private string _transferUrl = "https://someurl.com";

        [SetUp]
        public void Setup()
        {      
            _validParcel = new Parcel{
                VisitedHops = new List<HopArrival>{new HopArrival{
                    Code = "Code1",
                    Description = "Visited hops blabla",
                    DateTime = DateTime.Now.AddDays(-1)
                }},
                FutureHops = new List<HopArrival>{new HopArrival{
                    Code = _validTruckCode,
                    Description = "Future hops blabla",
                    DateTime = DateTime.Now.AddDays(1)
                },new HopArrival{
                    Code = _validWarehouseCode,
                    Description = "Future hops blabla",
                    DateTime = DateTime.Now.AddDays(1)
                },new HopArrival{
                    Code = _validTransferWarehouseCode,
                    Description = "Future hops blabla",
                    DateTime = DateTime.Now.AddDays(1)
                }},
                Recipient = new Recipient{
                    Country = "Österreich",
                    PostalCode = "A-1120",
                    Street = "Hauptstraße 12/12/12",
                    City = "Wien",
                    Name = "Josef Koch"
                },
                Sender = new Recipient{
                    Country = "Austria",
                    PostalCode = "A-1210",
                    Street = "Landstraße 27a",
                    City = "Wien",
                    Name = "Josef Wermann"
                },
                State = Parcel.StateEnum.InTransportEnum,
                TrackingId = _validTrackingId,
                Weight = 6.9f
            };

            var rootWarehouse = new DataAccess.Entities.Warehouse {
                Code = "Code1",
                IsRootWarehouse = true,
                HopType = "Warehouse",
                Level = 0,
                ProcessingDelayMins = 186,
                NextHops = new List<DataAccess.Entities.WarehouseNextHops>{
                    new DataAccess.Entities.WarehouseNextHops{
                        TraveltimeMins = 60,
                        Hop = new DataAccess.Entities.Warehouse{
                            Code = "Code2",
                            HopType = "Warehouse",
                            Level = 1,
                            ProcessingDelayMins = 100,
                            NextHops = new List<DataAccess.Entities.WarehouseNextHops>{
                                new DataAccess.Entities.WarehouseNextHops{
                                    TraveltimeMins = 60,
                                    Hop = new DataAccess.Entities.Truck{
                                        Code = "Code3",
                                        HopType = "Truck",
                                        ProcessingDelayMins = 50
                                    }
                                }    
                            }
                        }
                    },
                    new DataAccess.Entities.WarehouseNextHops{
                        TraveltimeMins = 80,
                        Hop = new DataAccess.Entities.Warehouse{
                            Code = "Code4",
                            HopType = "Warehouse",
                            Level = 1,
                            ProcessingDelayMins = 120,
                            NextHops = new List<DataAccess.Entities.WarehouseNextHops>{
                                new DataAccess.Entities.WarehouseNextHops{
                                    TraveltimeMins = 70,
                                    Hop = new DataAccess.Entities.Truck{
                                        Code = "Code5",
                                        HopType = "Truck",
                                        ProcessingDelayMins = 40
                                    }
                                }    
                            }
                        }
                    }
                }
            };

            var warehouses = new List<DataAccess.Entities.Warehouse>{
                rootWarehouse, (DataAccess.Entities.Warehouse)rootWarehouse.NextHops[0].Hop, (DataAccess.Entities.Warehouse)rootWarehouse.NextHops[1].Hop
            };
            var trucks = new List<DataAccess.Entities.Truck>{
                (DataAccess.Entities.Truck)((DataAccess.Entities.Warehouse)rootWarehouse.NextHops[0].Hop).NextHops[0].Hop,
                (DataAccess.Entities.Truck)((DataAccess.Entities.Warehouse)rootWarehouse.NextHops[1].Hop).NextHops[0].Hop
            };

            var webhookResponses = new List<DataAccess.Entities.WebhookResponse>{new DataAccess.Entities.WebhookResponse {
                TrackingId = _validTrackingId,
                CreatedAt = DateTime.Now,
                Id = 1,
                Url = "https://test.com"
            }};

            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DalMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();

            //mock parcel repository
            var mockParcelRepo = new Mock<IParcelRepository>();
            mockParcelRepo.Setup(parcelRepository => parcelRepository.ContainsTrackingId(
                _dupliicateTrackingId
            )).Returns(true);
            mockParcelRepo.Setup(parcelRepository => parcelRepository.ContainsTrackingId(
                _validTrackingId
            )).Returns(false);
            mockParcelRepo.Setup(parcelRepository => parcelRepository.GetParcelByTrackingId(
                _validTrackingId
            )).Returns(mapper.Map<DataAccess.Entities.Parcel>(_validParcel));
            mockParcelRepo.Setup(parcelRepository => parcelRepository.GetParcelByTrackingId(
                _notFoundTrackingId
            )).Throws(new DataAccess.Entities.DALNotFoundException("TrackingId Not Found"));
            mockParcelRepo.Setup(parcelRepository => parcelRepository.GetParcelByTrackingId(
                _invalidTrackingId
            )).Throws(new DataAccess.Entities.DALException("Invalid TrackingId"));
            mockParcelRepo.Setup(parcelRepository => parcelRepository.Create(
                It.IsAny<DataAccess.Entities.Parcel>()
            )).Returns<DataAccess.Entities.Parcel>(p => p.Id);
            mockParcelRepo.Setup(parcelRepository => parcelRepository.Update(
                It.IsAny<DataAccess.Entities.Parcel>()
            ));
            mockParcelRepo.Setup(parcelRepository => parcelRepository.Update(
                It.Is<DataAccess.Entities.Parcel>(p => p.TrackingId == _notFoundTrackingId)
            )).Throws(new DataAccess.Entities.DALNotFoundException("Parcel Not Found"));

            var mockWarehouseRepo = new Mock<IWarehouseRepository>();
            mockWarehouseRepo.Setup(warehouseRepo => warehouseRepo.GetAllTrucks()).Returns(trucks);
            mockWarehouseRepo.Setup(warehouseRepo => warehouseRepo.GetAllWarehouses()).Returns(warehouses);
            mockWarehouseRepo.Setup(warehouseRepo => warehouseRepo.GetRootWarehouse()).Returns(rootWarehouse);
            mockWarehouseRepo.Setup(warehouseRepo => warehouseRepo.GetHopByCoordinates(1, 1)).Returns((rootWarehouse.NextHops[0].Hop as DataAccess.Entities.Warehouse).NextHops[0].Hop);
            mockWarehouseRepo.Setup(warehouseRepo => warehouseRepo.GetHopByCoordinates(2, 2)).Returns((rootWarehouse.NextHops[1].Hop as DataAccess.Entities.Warehouse).NextHops[0].Hop);
            mockWarehouseRepo.Setup(warehouseRepo => warehouseRepo.GetHopByCode(_validTransferWarehouseCode))
                .Returns(new DataAccess.Entities.TransferWarehouse{
                    HopType = "TransferWarehouse",
                    LogisticsPartnerUrl = _transferUrl
                });
            mockWarehouseRepo.Setup(warehouseRepo => warehouseRepo.GetHopByCode(_validWarehouseCode))
                .Returns(new DataAccess.Entities.Warehouse{HopType = "Warehouse"});
            mockWarehouseRepo.Setup(warehouseRepo => warehouseRepo.GetHopByCode(_validTruckCode))
                .Returns(new DataAccess.Entities.Truck{HopType = "Truck"});

            var mockWebhookRepo = new Mock<IWebhookRepository>();
            mockWebhookRepo.Setup(webhookRepo => webhookRepo.GetByTrackingId(_validTrackingId)).Returns(webhookResponses);
            mockWebhookRepo.Setup(webhookRepo => webhookRepo.Delete(webhookResponses));
           
            var mockEncodingAgent = new Mock<IGeoEncodingAgent>();
            mockEncodingAgent.Setup(encodingAgent => encodingAgent.AddressEncoder($"{_validParcel.Sender.Street}, {_validParcel.Sender.PostalCode} {_validParcel.Sender.City}, {_validParcel.Sender.Country}"))
                .Returns(new GeoCoordinate{Lat = 1, Lon = 1});
            mockEncodingAgent.Setup(encodingAgent => encodingAgent.AddressEncoder($"{_validParcel.Recipient.Street}, {_validParcel.Recipient.PostalCode} {_validParcel.Recipient.City}, {_validParcel.Recipient.Country}"))
                .Returns(new GeoCoordinate{Lat = 2, Lon = 2});

            var mockWebhookAgent = new Mock<IWebhookAgent>();
            mockWebhookAgent.Setup(webhookAgent => webhookAgent.Notify(It.IsAny<IEnumerable<WebhookResponse>>(), It.IsAny<WebhookMessage>()));
            
            var loggerMock = new Mock<ILogger<TrackingLogic>>();

            var mockFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage{StatusCode = HttpStatusCode.OK});
            var client = new HttpClient(mockHttpMessageHandler.Object){
                BaseAddress = null
            };

            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            _trackingLogic = new TrackingLogic(mapper, mockParcelRepo.Object, mockWebhookRepo.Object, loggerMock.Object, mockEncodingAgent.Object, mockWarehouseRepo.Object, mockFactory.Object, mockWebhookAgent.Object);
        }

        [Test]
        public void Should_Transistion_Valid_Parcel()
        {
            var res = _trackingLogic.TransitionParcel(_validParcel, _validParcel.TrackingId);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<Parcel>(res);
        }

        [Test]
        public void Should_Throw_Exception_On_Invalid_Parcel()
        {
            _validParcel.Recipient = _validParcel.Sender;
            Assert.Throws<BLValidationException>(() => _trackingLogic.TransitionParcel(_validParcel, _validTrackingId));
        }

        [Test]
        public void Should_Throw_Exception_On_Transition_Parcel_Of_Null_TrackingId()
        {
            Assert.Throws<BLValidationException>(() => _trackingLogic.TransitionParcel(_validParcel, null));
        }

        [Test]
        public void Should_Throw_Exception_On_Transition_Parcel_Of_Null_Parcel()
        {
            Assert.Throws<BLValidationException>(() => _trackingLogic.TransitionParcel(null, _validTrackingId));
        }

        [Test]
        public void Should_Throw_Exception_On_Duplicate_TrackingId()
        {
            Assert.Throws<BLException>(() => _trackingLogic.TransitionParcel(_validParcel, _dupliicateTrackingId));
        }

        [Test]
        public void Should_Track_Parcel()
        {
            var res = _trackingLogic.TrackParcel(_validTrackingId);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<Parcel>(res);
        }

        [Test]
        public void Should_Throw_Not_Found_Exception_On_Invalid_TrackingId()
        {
            Assert.Throws<BLValidationException>(() => _trackingLogic.TrackParcel(_invalidCode));
        }

        [Test]
        public void Should_Throw_Not_Found_Exception_On_Track_Parcel()
        {
            Assert.Throws<BLNotFoundException>(() => _trackingLogic.TrackParcel(_notFoundTrackingId));
        }

        [Test]
        public void Should_Submit_Parcel()
        {            
            _validParcel.FutureHops = null;
            _validParcel.VisitedHops = null;
            _validParcel.TrackingId = null;
            _validParcel.State = null;

            var res = _trackingLogic.SubmitParcel(_validParcel);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<Parcel>(res);
            Assert.IsNotNull(res.TrackingId);
            Assert.IsNull(res.VisitedHops);
            Assert.IsNotNull(res.FutureHops);
            Assert.AreEqual(Parcel.StateEnum.PickupEnum, res.State);
            Assert.AreEqual("Code3", res.FutureHops[0].Code);
            Assert.AreEqual("Code2", res.FutureHops[1].Code);
            Assert.AreEqual("Code1", res.FutureHops[2].Code);
            Assert.AreEqual("Code4", res.FutureHops[3].Code);
            Assert.AreEqual("Code5", res.FutureHops[4].Code);
        }

        [Test]
        public void Should_Report_Parcel_Delivery()
        {
            Assert.DoesNotThrow(() => _trackingLogic.ReportParcelDelivery(_validTrackingId));
        }

        [Test]
        public void Should_Throw_Not_Found_Exception_On_Report_Parcel_Delivery()
        {
            Assert.Throws<BLNotFoundException>(() => _trackingLogic.ReportParcelDelivery(_notFoundTrackingId));
        }

        [Test]
        public void Should_Throw_Validation_Exception_On_Invalid_TrackingId()
        {
            Assert.Throws<BLValidationException>(() => _trackingLogic.ReportParcelDelivery(_invalidTrackingId));
        }

        [Test]
        public void Should_Report_Parcel_Hop_To_Warehouse()
        {
            Assert.DoesNotThrow(() => _trackingLogic.ReportParcelHop(_validTrackingId, _validWarehouseCode));
        }

        [Test]
        public void Should_Report_Parcel_Hop_To_TransferWarehouse()
        {
            Assert.DoesNotThrow(() => _trackingLogic.ReportParcelHop(_validTrackingId, _validTransferWarehouseCode));
        }

        [Test]
        public void Should_Report_Parcel_Hop_To_Truck()
        {
            Assert.DoesNotThrow(() => _trackingLogic.ReportParcelHop(_validTrackingId, _validTruckCode));
        }
        
        [Test]
        public void Should_Throw_Exception_On_Report_Parcel_Hop_Of_Invalid_Code()
        {
            Assert.Throws<BLValidationException>(() => _trackingLogic.ReportParcelHop(_validTrackingId, _invalidCode));
        }

        [Test]
        public void Should_Throw_Exception_On_Report_Parcel_Hop_Of_Not_Found_TrackingId()
        {
            Assert.Throws<BLNotFoundException>(() => _trackingLogic.ReportParcelHop(_notFoundTrackingId, _validWarehouseCode));
        }


    }
}