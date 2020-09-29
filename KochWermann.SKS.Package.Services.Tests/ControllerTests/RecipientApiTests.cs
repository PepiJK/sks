using NUnit.Framework;
using KochWermann.SKS.Package.Services.Controllers;
using KochWermann.SKS.Package.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace KochWermann.SKS.Package.Services.Tests.ControllerTests
{
    public class RecipientApiTests
    {
        private RecipientApiController recipientApiController;
        
        [SetUp]
        public void Setup()
        {
            recipientApiController = new RecipientApiController();
        }

        [Test]
        public void Should_Track_Parcel()
        {
            var res = recipientApiController.TrackParcel("PYJRB4HZ6") as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(200, res.StatusCode);
        }

        [Test]
        public void Should_Not_Track_Parcel()
        {
            var res = recipientApiController.TrackParcel(null) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(404, res.StatusCode);
        }
    }
}