using NUnit.Framework;
using KochWermann.SKS.Package.Services.Controllers;
using KochWermann.SKS.Package.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;
using KochWermann.SKS.Package.Services.Mapper;

namespace KochWermann.SKS.Package.Services.Tests.ControllerTests
{
    public class LogisticsPartnerApiTests
    {
        private LogisticsPartnerApiController logisticsPartnerController;
        
        [SetUp]
        public void Setup()
        {
            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BlMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();

            logisticsPartnerController = new LogisticsPartnerApiController(mapper);
        }

        [Test]
        public void Should_Transistion_Parcel()
        {
            var res = logisticsPartnerController.TransitionParcel(new Parcel(), "PYJRB4HZ6") as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(200, res.StatusCode);
        }

        [Test]
        public void Should_Not_Transistion_Parcel()
        {
            var res = logisticsPartnerController.TransitionParcel(null, null) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(400, res.StatusCode);
        }
    }
}