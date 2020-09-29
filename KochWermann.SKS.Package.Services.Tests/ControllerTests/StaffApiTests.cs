using NUnit.Framework;
using KochWermann.SKS.Package.Services.Controllers;
using KochWermann.SKS.Package.Services.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace KochWermann.SKS.Package.Services.Tests.ControllerTests
{
    public class StaffApiTests
    {
        private StaffApiController staffApiController;
        
        [SetUp]
        public void Setup()
        {
            staffApiController = new StaffApiController();
        }

        [Test]
        public void Should_Report_Parcel_Delivery()
        {
            var res = staffApiController.ReportParcelDelivery("PYJRB4HZ6") as StatusCodeResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(200, res.StatusCode);
        }

        [Test]
        public void Should_Not_Report_Parcel_Delivery()
        {
            var res = staffApiController.ReportParcelDelivery(null) as StatusCodeResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(404, res.StatusCode);
        }

        [Test]
        public void Should_Report_Parcel_Hop()
        {
            var res = staffApiController.ReportParcelHop("PYJRB4HZ6", "TEST\\d") as StatusCodeResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(200, res.StatusCode);
        }

        [Test]
        public void Should_Not_Report_Parcel_Hop()
        {
            var res = staffApiController.ReportParcelHop(null, null) as StatusCodeResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(404, res.StatusCode);
        }
    }
}