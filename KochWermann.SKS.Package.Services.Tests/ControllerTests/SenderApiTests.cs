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
using Microsoft.Extensions.Logging;

namespace KochWermann.SKS.Package.Services.Tests.ControllerTests
{
    public class SenderApiTests
    {
        private SenderApiController _senderApiController;
        private Parcel _testParcel;
        
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
            var recipients = Builder<Recipient>.CreateListOfSize(2).Build();
            _testParcel = Builder<Parcel>.CreateNew()
                .With(x => x.Recipient = recipients[0])
                .With(x => x.Sender = recipients[1])
            .Build();

            //mock tracking logic
            var mock = new Mock<ITrackingLogic>();
            mock.Setup(trackingLogic => trackingLogic.SubmitParcel(
                It.IsAny<BusinessLogic.Entities.Parcel>()
            )).Returns(new BusinessLogic.Entities.Parcel());
            mock.Setup(trackingLogic => trackingLogic.SubmitParcel(
                It.Is<BusinessLogic.Entities.Parcel>(p => p.Recipient == null)
            )).Throws(new BusinessLogic.Entities.BLException("Invalid Parcel", new System.Exception()));
            
            var loggerMock = new Mock<ILogger<SenderApiController>>();

            _senderApiController = new SenderApiController(mapper, mock.Object, loggerMock.Object);
        }

        [Test]
        public void Should_Return_Ok_On_Submit_Parcel()
        {
            var res = _senderApiController.SubmitParcel(_testParcel);
            Assert.IsInstanceOf<OkObjectResult>(res);
            Assert.IsInstanceOf<Services.DTOs.NewParcelInfo>((res as OkObjectResult).Value);
        }

        [Test]
        public void Should_Return_Bad_Request_On_Submit_Parcel_Of_Null_Parcel()
        {
            var res = _senderApiController.SubmitParcel(null);
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
            Assert.IsInstanceOf<Services.DTOs.Error>((res as BadRequestObjectResult).Value);
        }

        [Test]
        public void Should_Return_Bad_Request_On_Submit_Parcel_Of_Invalid_Parcel()
        {
            _testParcel.Recipient = null;
            var res = _senderApiController.SubmitParcel(_testParcel);
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
            Assert.IsInstanceOf<Services.DTOs.Error>((res as BadRequestObjectResult).Value);
        }
    }
}