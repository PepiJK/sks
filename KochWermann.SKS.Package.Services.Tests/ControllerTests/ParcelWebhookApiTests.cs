using NUnit.Framework;
using KochWermann.SKS.Package.Services.Controllers;
using KochWermann.SKS.Package.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace KochWermann.SKS.Package.Services.Tests.ControllerTests
{
    public class ParcelWebhookApiTests
    {
        private ParcelWebhookApiController _parcelWebhookApiController;
        
        [SetUp]
        public void Setup()
        {
            _parcelWebhookApiController = new ParcelWebhookApiController();
        }

        [Test]
        public void Should_Return_Ok_On_List_Parcel_Webhooks()
        {
            var res = _parcelWebhookApiController.ListParcelWebhooks("PYJRB4HZ6");
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<OkObjectResult>(res);
        }

        [Test]
        public void Should_Return_Not_Found_On_List_Parcel_Webhooks()
        {
            var res = _parcelWebhookApiController.ListParcelWebhooks(null);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<NotFoundResult>(res);
        }

        [Test]
        public void Should_Return_Ok_On_Subscribe_Parcel_Webhook()
        {
            var res = _parcelWebhookApiController.SubscribeParcelWebhook("PYJRB4HZ6", "https://www.google.com");
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<OkObjectResult>(res);
        }

        [Test]
        public void Should_Return_Not_Found_On_Subscribe_Parcel_Webhook()
        {
            var res = _parcelWebhookApiController.SubscribeParcelWebhook(null, null);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<NotFoundResult>(res);
        }

        [Test]
        public void Should_Return_Ok_On_Unsubscribe_Parcel_Webhook()
        {
            var res = _parcelWebhookApiController.UnsubscribeParcelWebhook(123);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<OkResult>(res);
        }

        [Test]
        public void Should_Return_Not_Found_On_Unsubscribe_Parcel_Webhook()
        {
            var res = _parcelWebhookApiController.UnsubscribeParcelWebhook(null);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<NotFoundResult>(res);
        }
    }
}