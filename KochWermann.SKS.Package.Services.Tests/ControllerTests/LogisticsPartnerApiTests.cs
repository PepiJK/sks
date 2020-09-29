using NUnit.Framework;
using KochWermann.SKS.Package.Services.Controllers;
using KochWermann.SKS.Package.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace KochWermann.SKS.Package.Services.Tests.ControllerTests
{
    public class LogisticsPartnerApiTests
    {
        private LogisticsPartnerApiController logisticsPartnerController;
        
        [SetUp]
        public void Setup()
        {
            logisticsPartnerController = new LogisticsPartnerApiController();
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