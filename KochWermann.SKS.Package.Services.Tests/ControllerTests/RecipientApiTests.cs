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
using System.Collections.Generic;

namespace KochWermann.SKS.Package.Services.Tests.ControllerTests
{
    public class RecipientApiTests
    {
        private RecipientApiController _recipientApiController;
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

            //mock tracking logic
            var mock = new Mock<ITrackingLogic>();
            mock.Setup(trackingLogic => trackingLogic.TrackParcel(
                It.IsRegex("^[A-Z0-9]{9}$")
            )).Returns(new BusinessLogic.Entities.Parcel());

            //create api controller instance
            _recipientApiController = new RecipientApiController(mapper, mock.Object);
        }

        [Test]
        public void Should_Track_Parcel()
        {
            var res = _recipientApiController.TrackParcel(_testTrackingId);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<OkObjectResult>(res);
            Assert.IsInstanceOf<Services.DTOs.TrackingInformation>((res as OkObjectResult).Value);
        }

        [Test]
        public void Should_Not_Track_Parcel()
        {
            var res = _recipientApiController.TrackParcel(null);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<NotFoundResult>(res);
        }
    }
}