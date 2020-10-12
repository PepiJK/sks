using System;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using NUnit.Framework;

namespace KochWermann.SKS.Package.BusinessLogic.Tests
{
    public class WebhookLogicTests
    {
        private IWebhookLogic _webhookLogic = new WebhookLogic();

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Should_Throw_Not_Implemented_Exception_On_List_Parcel_Webhooks()
        {
            Assert.Throws<NotImplementedException>(() => _webhookLogic.ListParcelWebhooks(null));
        }

        [Test]
        public void Should_Throw_Not_Implemented_Exception_On_Subscribe_Parcel_Webhook()
        {
            Assert.Throws<NotImplementedException>(() => _webhookLogic.SubscribeParcelWebhook(null, null));
        }

        [Test]
        public void Should_Throw_Not_Implemented_Exception_On_Unsubscribe_Parcel_Webhooks()
        {
            Assert.Throws<NotImplementedException>(() => _webhookLogic.UnsubscribeParcelWebhook(0));
        }
    }
}