using NUnit.Framework;
using KochWermann.SKS.Package.Services.Controllers;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using KochWermann.SKS.Package.Services.Mapper;
using Moq;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using Microsoft.Extensions.Logging;

namespace KochWermann.SKS.Package.Services.Tests.ControllerTests
{
    public class RecipientApiTests
    {
        private RecipientApiController _recipientApiController;
        private string _testTrackingId = "PYJRB4HZ6";
        private string _notFoundTrackingId = "PYJRB4HZ9";
        private string _invalidTrackingId = "hallo";
        
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
            mock.Setup(trackingLogic => trackingLogic.TrackParcel(
                _testTrackingId
            )).Returns(new BusinessLogic.Entities.Parcel());
            mock.Setup(trackingLogic => trackingLogic.TrackParcel(
                _notFoundTrackingId
            )).Throws(new BusinessLogic.Entities.BLNotFoundException("Not Found", new System.Exception()));
            mock.Setup(trackingLogic => trackingLogic.TrackParcel(
                _invalidTrackingId
            )).Throws(new BusinessLogic.Entities.BLException("Invalid TrackingId", new System.Exception()));


            var loggerMock = new Mock<ILogger<RecipientApiController>>();
            
            //create api controller instance
            _recipientApiController = new RecipientApiController(mapper, mock.Object, loggerMock.Object);
        }

        [Test]
        public void Should_Return_Ok_On_Track_Parcel()
        {
            var res = _recipientApiController.TrackParcel(_testTrackingId);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<OkObjectResult>(res);
            Assert.IsInstanceOf<Services.DTOs.TrackingInformation>((res as OkObjectResult).Value);
        }

        [Test]
        public void Should_Return_Bad_Request_On_Track_Parcel_On_Null_TrackingId()
        {
            var res = _recipientApiController.TrackParcel(null);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
        }

        [Test]
        public void Should_Return_Bad_Request_On_Track_Parcel_On_Invalid_TrackingId()
        {
            var res = _recipientApiController.TrackParcel(_invalidTrackingId);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
        }

        [Test]
        public void Should_Return_Not_Found_On_On_Track_Parcel()
        {
            var res = _recipientApiController.TrackParcel(_notFoundTrackingId);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<NotFoundObjectResult>(res);
            Assert.IsInstanceOf<Services.DTOs.Error>((res as NotFoundObjectResult).Value);
        }
    }
}