using System;
using AutoMapper;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using KochWermann.SKS.Package.BusinessLogic.Mapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace KochWermann.SKS.Package.BusinessLogic.Tests
{
    public class WebhookLogicTests
    {
        private WebhookLogic _webhookLogic;

        [SetUp]
        public void Setup()
        {
            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DalMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var loggerMock = new Mock<ILogger<WebhookLogic>>();

            _webhookLogic = new WebhookLogic(mapper, loggerMock.Object);
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