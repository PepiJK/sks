using NUnit.Framework;
using KochWermann.SKS.Package.Services.Controllers;
using KochWermann.SKS.Package.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using KochWermann.SKS.Package.Services.Mapper;
using AutoMapper;
using Moq;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;

namespace KochWermann.SKS.Package.Services.Tests.ControllerTests
{
    public class StaffApiTests
    {
        private StaffApiController _staffApiController;
        private string _testTrackingId = "PYJRB4HZ6";
        private string _testCode = "TEST\\d";
        
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
                It.IsRegex("^[A-Z0-9]{9}$")
            ));
            mock.Setup(trackingLogic => trackingLogic.ReportParcelHop(
                It.IsRegex("^[A-Z0-9]{9}$"),
                It.IsRegex("/^[A-Z]{4}\\d{1,4}$/")
            ));

            _staffApiController = new StaffApiController(mapper, mock.Object);
        }

        [Test]
        public void Should_Report_Parcel_Delivery()
        {
            var res = _staffApiController.ReportParcelDelivery(_testTrackingId);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<OkResult>(res);
        }

        [Test]
        public void Should_Not_Report_Parcel_Delivery()
        {
            var res = _staffApiController.ReportParcelDelivery(null);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<NotFoundResult>(res);
        }

        [Test]
        public void Should_Report_Parcel_Hop()
        {
            var res = _staffApiController.ReportParcelHop(_testTrackingId, _testCode);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<OkResult>(res);
        }

        [Test]
        public void Should_Not_Report_Parcel_Hop()
        {
            var res = _staffApiController.ReportParcelHop(null, null) as StatusCodeResult;
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<NotFoundResult>(res);
        }
    }
}