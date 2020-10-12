using NUnit.Framework;
using KochWermann.SKS.Package.Services.Controllers;
using KochWermann.SKS.Package.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;
using KochWermann.SKS.Package.Services.Mapper;
using Moq;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using FizzWare.NBuilder;

namespace KochWermann.SKS.Package.Services.Tests.ControllerTests
{
    public class LogisticsPartnerApiTests
    {
        private LogisticsPartnerApiController _logisticsPartnerApiController;
        private Parcel _testServiceParcel;
        private string _testTrackingId = "PYJRB4HZ6";
        
        [SetUp]
        public void Setup()
        {
            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BlMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();

            //generate test objects
            var recipients = Builder<Services.DTOs.Recipient>.CreateListOfSize(2).Build();
            _testServiceParcel = Builder<Services.DTOs.Parcel>.CreateNew()
                .With(x => x.Recipient = recipients[0])
                .With(x => x.Sender = recipients[1])
            .Build();

            //mock tracking logic
            var mock = new Mock<ITrackingLogic>();
            mock.Setup(trackingLogic => trackingLogic.TransitionParcel(
                It.IsAny<BusinessLogic.Entities.Parcel>(),
                It.IsRegex("^[A-Z0-9]{9}$")
            )).Returns(new BusinessLogic.Entities.Parcel());

            //create api controller instance
            _logisticsPartnerApiController = new LogisticsPartnerApiController(mapper, mock.Object);
        }

        [Test]
        public void Should_Return_Ok_On_Transition_Parcel()
        {
            var res = _logisticsPartnerApiController.TransitionParcel(_testServiceParcel, _testTrackingId);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<OkObjectResult>(res);
            Assert.IsInstanceOf<Services.DTOs.NewParcelInfo>((res as OkObjectResult).Value);
        }

        [Test]
        public void Should_Return_Bad_Request_On_Transition_Parcel()
        {
            var res = _logisticsPartnerApiController.TransitionParcel(null, null);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
            Assert.IsInstanceOf<Services.DTOs.Error>((res as BadRequestObjectResult).Value);
        }
    }
}