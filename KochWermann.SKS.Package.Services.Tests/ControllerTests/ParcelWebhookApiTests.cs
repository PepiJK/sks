using NUnit.Framework;
using KochWermann.SKS.Package.Services.Controllers;
using KochWermann.SKS.Package.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace KochWermann.SKS.Package.Services.Tests.ControllerTests
{
    public class ParcelWebhookApiTests
    {
        private ParcelWebhookApiController parcelWebhookApiController;
        
        [SetUp]
        public void Setup()
        {
            parcelWebhookApiController = new ParcelWebhookApiController();
        }

        [Test]
        public void Should_List_Parcel_Webhooks()
        {
            var res = parcelWebhookApiController.ListParcelWebhooks("PYJRB4HZ6") as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(200, res.StatusCode);
        }

        [Test]
        public void Should_Not_List_Parcel_Webhooks()
        {
            var res = parcelWebhookApiController.ListParcelWebhooks(null) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(404, res.StatusCode);
        }

        [Test]
        public void Should_Subscribe_Parcel_Webhook()
        {
            var res = parcelWebhookApiController.SubscribeParcelWebhook("PYJRB4HZ6", "https://www.google.com") as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(200, res.StatusCode);
        }

        [Test]
        public void Should_Not_Subscribe_Parcel_Webhook()
        {
            var res = parcelWebhookApiController.SubscribeParcelWebhook(null, null) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(404, res.StatusCode);
        }

        [Test]
        public void Should_Unsubscribe_Parcel_Webhook()
        {
            var res = parcelWebhookApiController.UnsubscribeParcelWebhook(123) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(200, res.StatusCode);
        }

        [Test]
        public void Should_Not_Unsubscribe_Parcel_Webhook()
        {
            var res = parcelWebhookApiController.UnsubscribeParcelWebhook(null) as IStatusCodeActionResult;
            Assert.IsNotNull(res);
            Assert.AreEqual(404, res.StatusCode);
        }
    }
}