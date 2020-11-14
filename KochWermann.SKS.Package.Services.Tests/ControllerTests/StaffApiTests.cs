using NUnit.Framework;
using KochWermann.SKS.Package.Services.Controllers;
using KochWermann.SKS.Package.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using KochWermann.SKS.Package.Services.Mapper;
using AutoMapper;
using Moq;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using Microsoft.Extensions.Logging;

namespace KochWermann.SKS.Package.Services.Tests.ControllerTests
{
    public class StaffApiTests
    {
        private StaffApiController _staffApiController;
        private string _testTrackingId = "PYJRB4HZ6";
        private string _notFoundTrackingId = "PYJRB4HZ9";
        private string _invalidTrackingId = "hallo";
        private string _testCode = "TEST1234";
        private string _notFoundCode = "TEST9878";
        private string _invalidCode = "hallo";
        
        [SetUp]
        public void Setup()
        {
            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BlMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();

            //mock tracking logic
            var mock = new Mock<ITrackingLogic>();
            mock.Setup(trackingLogic => trackingLogic.ReportParcelDelivery(
                _testTrackingId
            ));
            mock.Setup(trackingLogic => trackingLogic.ReportParcelDelivery(
                _notFoundTrackingId
            )).Throws(new BusinessLogic.Entities.BLNotFoundException("TrackingId Not Found", new System.Exception()));
            mock.Setup(trackingLogic => trackingLogic.ReportParcelDelivery(
                _invalidTrackingId
            )).Throws(new BusinessLogic.Entities.BLException("Invalid TrackingId", new System.Exception()));
            
            mock.Setup(trackingLogic => trackingLogic.ReportParcelHop(
                _testTrackingId,
                _testCode
            ));
            mock.Setup(trackingLogic => trackingLogic.ReportParcelHop(
                _testTrackingId,
                _notFoundCode
            )).Throws(new BusinessLogic.Entities.BLNotFoundException("Code Not Found", new System.Exception()));
            mock.Setup(trackingLogic => trackingLogic.ReportParcelHop(
                _testTrackingId,
                _invalidCode
            )).Throws(new BusinessLogic.Entities.BLException("Invalid Code", new System.Exception()));

            var loggerMock = new Mock<ILogger<StaffApiController>>();

            _staffApiController = new StaffApiController(mapper, mock.Object, loggerMock.Object);
        }

        [Test]
        public void Should_Return_Ok_On_Report_Parcel_Delivery()
        {
            var res = _staffApiController.ReportParcelDelivery(_testTrackingId);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<OkObjectResult>(res);
        }

        [Test]
        public void Should_Return_Bad_Request_On_Report_Parcel_Delivery_Of_Null_TrackingId()
        {
            var res = _staffApiController.ReportParcelDelivery(null);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
            Assert.IsInstanceOf<Services.DTOs.Error>((res as BadRequestObjectResult).Value);
        }

        [Test]
        public void Should_Return_Bad_Request_On_Report_Parcel_Delivery_Of_Invalid_TrackingId()
        {
            var res = _staffApiController.ReportParcelDelivery(_invalidTrackingId);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
            Assert.IsInstanceOf<Services.DTOs.Error>((res as BadRequestObjectResult).Value);
        }

        [Test]
        public void Should_Return_Not_Found_On_Report_Parcel_Delivery_Of_Not_Found_TrackingId()
        {
            var res = _staffApiController.ReportParcelDelivery(_notFoundTrackingId);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<NotFoundObjectResult>(res);
            Assert.IsInstanceOf<Services.DTOs.Error>((res as NotFoundObjectResult).Value);
        }

        [Test]
        public void Should_Return_Ok_On_Report_Parcel_Hop()
        {
            var res = _staffApiController.ReportParcelHop(_testTrackingId, _testCode);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<OkObjectResult>(res);
        }

        [Test]
        public void Should_Return_Bad_Request_On_Report_Parcel_Hop_On_Null()
        {
            var res = _staffApiController.ReportParcelHop(null, null);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
        }

        [Test]
        public void Should_Return_Bad_Request_On_Report_Parcel_Hop_Of_Invalid_Code()
        {
            var res = _staffApiController.ReportParcelHop(_testTrackingId, _invalidCode);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
            Assert.IsInstanceOf<Services.DTOs.Error>((res as BadRequestObjectResult).Value);
        }

        [Test]
        public void Should_Return_Not_Found_On_Report_Parcel_Hop_Of_Not_Found_Code()
        {
            var res = _staffApiController.ReportParcelHop(_testTrackingId, _notFoundCode);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<NotFoundObjectResult>(res);
            Assert.IsInstanceOf<Services.DTOs.Error>((res as NotFoundObjectResult).Value);
        }
    }
}