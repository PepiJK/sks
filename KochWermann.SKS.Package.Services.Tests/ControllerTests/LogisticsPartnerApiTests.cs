using NUnit.Framework;
using KochWermann.SKS.Package.Services.Controllers;
using KochWermann.SKS.Package.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;
using KochWermann.SKS.Package.Services.Mapper;
using Moq;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;

namespace KochWermann.SKS.Package.Services.Tests.ControllerTests
{
    public class LogisticsPartnerApiTests
    {
        private LogisticsPartnerApiController _logisticsPartnerApiController;
        
        [SetUp]
        public void Setup()
        {
            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BlMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();

            //moq configuration
            var mock = new Mock<ITrackingLogic>();
            mock.Setup(trackingLogic => trackingLogic.TransitionParcel(It.IsAny<BusinessLogic.Entities.Parcel>(), It.IsRegex("^[A-Z0-9]{9}$"))).Returns(new BusinessLogic.Entities.Parcel());

            _logisticsPartnerApiController = new LogisticsPartnerApiController(mapper, mock.Object);
        }

        [Test]
        public void Should_Transistion_Parcel()
        {
            var res = _logisticsPartnerApiController.TransitionParcel(new Parcel(), "PYJRB4HZ6");
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<OkObjectResult>(res);
            Assert.IsInstanceOf<NewParcelInfo>((res as OkObjectResult).Value);
        }

        [Test]
        public void Should_Not_Transistion_Parcel()
        {
            var res = _logisticsPartnerApiController.TransitionParcel(null, null);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
            Assert.IsInstanceOf<Error>((res as BadRequestObjectResult).Value);
        }
    }
}