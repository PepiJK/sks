using NUnit.Framework;
using KochWermann.SKS.Package.Services.Controllers;
using KochWermann.SKS.Package.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;
using KochWermann.SKS.Package.Services.Mapper;

namespace KochWermann.SKS.Package.Services.Tests.ControllerTests
{
    public class SenderApiTests
    {
        private SenderApiController senderApiController;
        
        [SetUp]
        public void Setup()
        {
            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BlMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();

            senderApiController = new SenderApiController(mapper);
        }

        [Test]
        public void Should_Submit_Parcel()
        {
            var res = senderApiController.SubmitParcel(new Parcel()) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(200, res.StatusCode);
        }

        [Test]
        public void Should_Not_Submit_Parcel()
        {
            var res = senderApiController.SubmitParcel(null) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(400, res.StatusCode);
        }
    }
}