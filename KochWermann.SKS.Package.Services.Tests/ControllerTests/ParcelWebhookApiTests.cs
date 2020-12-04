using NUnit.Framework;
using KochWermann.SKS.Package.Services.Controllers;
using KochWermann.SKS.Package.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;
using KochWermann.SKS.Package.Services.Mapper;
using Moq;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using KochWermann.SKS.Package.BusinessLogic.Entities;

namespace KochWermann.SKS.Package.Services.Tests.ControllerTests
{
    public class ParcelWebhookApiTests
    {
        private ParcelWebhookApiController _parcelWebhookApiController;
        private string _validTrackingId = "PYJRB4HZ6";
        private string _notFoundTrackingId = "PYJRB4HZ9";
        private string _invalidTrackingId = "hallo";
        private string _validUrl = "https://test.com";
        private long _id = 1;
        
        [SetUp]
        public void Setup()
        {
            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BlMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var webhookResponses = new List<BusinessLogic.Entities.WebhookResponse>{new BusinessLogic.Entities.WebhookResponse{
                CreatedAt = DateTime.Now,
                Id = _id,
                TrackingId = _validTrackingId,
                Url = _validUrl
            }};

            var mockWebhookLogic = new Mock<IWebhookLogic>();
            mockWebhookLogic.Setup(webhookLogic => webhookLogic.ListParcelWebhooks(_validTrackingId)).Returns(webhookResponses);
            mockWebhookLogic.Setup(webhookLogic => webhookLogic.ListParcelWebhooks(_notFoundTrackingId)).Throws(new BLNotFoundException("not found", new Exception()));
            mockWebhookLogic.Setup(webhookLogic => webhookLogic.ListParcelWebhooks(_invalidTrackingId)).Throws(new BLValidationException("invalid", new Exception()));
            mockWebhookLogic.Setup(webhookLogic => webhookLogic.SubscribeParcelWebhook(_validTrackingId, _validUrl)).Returns(webhookResponses[0]);
            mockWebhookLogic.Setup(webhookLogic => webhookLogic.SubscribeParcelWebhook(_notFoundTrackingId, _validUrl)).Throws(new BLNotFoundException("not found", new Exception()));
            mockWebhookLogic.Setup(webhookLogic => webhookLogic.SubscribeParcelWebhook(_invalidTrackingId, _validUrl)).Throws(new BLValidationException("invalid", new Exception()));
            mockWebhookLogic.Setup(webhookLogic => webhookLogic.UnsubscribeParcelWebhook(_id));
            mockWebhookLogic.Setup(webhookLogic => webhookLogic.UnsubscribeParcelWebhook(0)).Throws(new BLNotFoundException("not found", new Exception()));

            var loggerMock = new Mock<ILogger<ParcelWebhookApiController>>();
            
            _parcelWebhookApiController = new ParcelWebhookApiController(mockWebhookLogic.Object, mapper, loggerMock.Object);
        }

        [Test]
        public void Should_Return_Ok_On_List_Parcel_Webhooks()
        {
            var res = _parcelWebhookApiController.ListParcelWebhooks(_validTrackingId);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<OkObjectResult>(res);
            Assert.IsInstanceOf<IEnumerable<Services.DTOs.WebhookResponse>>((res as OkObjectResult).Value);
        }

        [Test]
        public void Should_Return_Bad_Request_On_List_Parcel_Webhooks_Of_Null_TrackingId()
        {
            var res = _parcelWebhookApiController.ListParcelWebhooks(null);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
        }

        [Test]
        public void Should_Return_Bad_Request_On_List_Parcel_Webhooks_Of_Invalid_TrackingId()
        {
            var res = _parcelWebhookApiController.ListParcelWebhooks(_invalidTrackingId);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
        }

        [Test]
        public void Should_Return_Not_Found_On_List_Parcel_Webhooks()
        {
            var res = _parcelWebhookApiController.ListParcelWebhooks(_notFoundTrackingId);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<NotFoundObjectResult>(res);
        }

        [Test]
        public void Should_Return_Ok_On_Subscribe_Parcel_Webhook()
        {
            var res = _parcelWebhookApiController.SubscribeParcelWebhook(_validTrackingId, _validUrl);

            Assert.IsInstanceOf<OkObjectResult>(res);
            Assert.IsInstanceOf<Services.DTOs.WebhookResponse>((res as OkObjectResult).Value);
        }

        [Test]
        public void Should_Return_Bad_Request_On_Subscribe_Parcel_Webhook_Of_Null_TrackingId_And_Url()
        {
            var res = _parcelWebhookApiController.SubscribeParcelWebhook(null, null);

            Assert.IsInstanceOf<BadRequestObjectResult>(res);
        }

        [Test]
        public void Should_Return_Bad_Request_On_Subscribe_Parcel_Webhook_Of_Invalid_TrackingId()
        {
            var res = _parcelWebhookApiController.SubscribeParcelWebhook(_invalidTrackingId, _validUrl);
            
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
        }

        [Test]
        public void Should_Return_Not_Found_On_Subscribe_Parcel_Webhook()
        {
            var res = _parcelWebhookApiController.SubscribeParcelWebhook(_notFoundTrackingId, _validUrl);
            
            Assert.IsInstanceOf<NotFoundObjectResult>(res);
        }

        [Test]
        public void Should_Return_Ok_On_Unsubscribe_Parcel_Webhook()
        {
            var res = _parcelWebhookApiController.UnsubscribeParcelWebhook(_id);

            Assert.IsInstanceOf<OkObjectResult>(res);
        }

        [Test]
        public void Should_Return_Bad_Request_On_Unsubscribe_Parcel_Webhook()
        {
            var res = _parcelWebhookApiController.UnsubscribeParcelWebhook(null);

            Assert.IsInstanceOf<BadRequestObjectResult>(res);
        }

        [Test]
        public void Should_Return_Not_Found_On_Unsubscribe_Parcel_Webhook()
        {
            var res = _parcelWebhookApiController.UnsubscribeParcelWebhook(0);

            Assert.IsInstanceOf<NotFoundObjectResult>(res);
        }
    }
}