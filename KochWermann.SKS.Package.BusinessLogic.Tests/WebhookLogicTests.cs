using System;
using System.Collections.Generic;
using AutoMapper;
using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using KochWermann.SKS.Package.BusinessLogic.Mapper;
using KochWermann.SKS.Package.DataAccess.Entities;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace KochWermann.SKS.Package.BusinessLogic.Tests
{
    public class WebhookLogicTests
    {
        private WebhookLogic _webhookLogic;
        private string _validTrackingId = "PYJRB4HZ6";
        private string _notFoundTrackingId = "PYJRB4HZ9";
        private string _invalidTrackingId = "hallo";
        private string _validUrl = "https://test.com";
        private string _invalidUrl = "https:/test.com";

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

            var webhookResponses = new List<DataAccess.Entities.WebhookResponse>{new DataAccess.Entities.WebhookResponse {
                TrackingId = _validTrackingId,
                CreatedAt = DateTime.Now,
                Id = 1,
                Url = "https://test.com"
            }};

            var mockWebhookRepo = new Mock<IWebhookRepository>();
            mockWebhookRepo.Setup(webhookRepo => webhookRepo.GetByTrackingId(_validTrackingId)).Returns(webhookResponses);
            mockWebhookRepo.Setup(webhookRepo => webhookRepo.GetByTrackingId(_notFoundTrackingId)).Throws(new DALNotFoundException("not found"));
            mockWebhookRepo.Setup(webhookRepo => webhookRepo.Create(It.IsAny<DataAccess.Entities.WebhookResponse>())).Returns(1);
            mockWebhookRepo.Setup(webhookRepo => webhookRepo.GetById(1)).Returns(webhookResponses[0]);
            mockWebhookRepo.Setup(webhookRepo => webhookRepo.GetById(0)).Throws(new DALNotFoundException("not found"));
            mockWebhookRepo.Setup(webhookRepo => webhookRepo.Delete(1));
            mockWebhookRepo.Setup(webhookRepo => webhookRepo.Delete(0)).Throws(new DALNotFoundException("not found"));

            var mockParcelRepo = new Mock<IParcelRepository>();
            mockParcelRepo.Setup(parcelRepo => parcelRepo.ContainsTrackingId(_validTrackingId)).Returns(true);
            mockParcelRepo.Setup(parcelRepo => parcelRepo.ContainsTrackingId(_notFoundTrackingId)).Returns(false);

            _webhookLogic = new WebhookLogic(mockWebhookRepo.Object, mockParcelRepo.Object, mapper, loggerMock.Object);
        }

        [Test]
        public void Should_List_Parcel_Webhooks()
        {
            var webhooks = _webhookLogic.ListParcelWebhooks(_validTrackingId);

            Assert.IsInstanceOf<IEnumerable<Entities.WebhookResponse>>(webhooks);
        }

        [Test]
        public void Should_Throw_Not_Found_On_List_Parcel_Webhooks()
        {
            Assert.Throws<BLNotFoundException>(() => _webhookLogic.ListParcelWebhooks(_notFoundTrackingId));
        }

         [Test]
        public void Should_Throw_Validation_Exception_On_List_Parcel_Webhooks()
        {
            Assert.Throws<BLValidationException>(() => _webhookLogic.ListParcelWebhooks(_invalidTrackingId));
        }

        [Test]
        public void Should_Subscribe_Parcel_Webhook()
        {
            var webhook = _webhookLogic.SubscribeParcelWebhook(_validTrackingId, _validUrl);

            Assert.IsInstanceOf<Entities.WebhookResponse>(webhook);
        }

        [Test]
        public void Should_Throw_Not_Found_On_Subscribe_Parcel_Webhook()
        {
            Assert.Throws<BLNotFoundException>(() => _webhookLogic.SubscribeParcelWebhook(_notFoundTrackingId, _validUrl));
        }

        [Test]
        public void Should_Throw_Validation_Exception_On_Subscribe_Parcel_Webhook()
        {
            Assert.Throws<BLValidationException>(() => _webhookLogic.SubscribeParcelWebhook(_validTrackingId, _invalidUrl));
        }

        [Test]
        public void Should_Unsubscribe_Parcel_Webhooks()
        {
            Assert.DoesNotThrow(() => _webhookLogic.UnsubscribeParcelWebhook(1));
        }

        [Test]
        public void Should_Throw_Not_Found_On_Unsubscribe_Parcel_Webhooks()
        {
            Assert.Throws<BLNotFoundException>(() => _webhookLogic.UnsubscribeParcelWebhook(0));
        }
    }
}