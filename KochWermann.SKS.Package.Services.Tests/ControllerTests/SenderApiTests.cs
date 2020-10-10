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
    public class SenderApiTests
    {
        private SenderApiController _senderApiController;
        
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
            mock.Setup(trackingLogic => trackingLogic.SubmitParcel(It.IsAny<BusinessLogic.Entities.Parcel>())).Returns(new BusinessLogic.Entities.Parcel());

            _senderApiController = new SenderApiController(mapper, mock.Object);
        }

        [Test]
        public void Should_Submit_Parcel()
        {
            var res = _senderApiController.SubmitParcel(new Parcel());
            Assert.IsInstanceOf<OkObjectResult>(res);
            Assert.IsInstanceOf<NewParcelInfo>((res as OkObjectResult).Value);
        }

        [Test]
        public void Should_Not_Submit_Parcel()
        {
            var res = _senderApiController.SubmitParcel(null);
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
            Assert.IsInstanceOf<Error>((res as BadRequestObjectResult).Value);
        }
    }
}