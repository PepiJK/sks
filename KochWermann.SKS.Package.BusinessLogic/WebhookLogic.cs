using System;
using AutoMapper;
using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using Microsoft.Extensions.Logging;

namespace KochWermann.SKS.Package.BusinessLogic
{
    public class WebhookLogic : IWebhookLogic
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public WebhookLogic (IMapper mapper, ILogger<WebhookLogic> logger)
        {
            _mapper = mapper;
            _logger = logger;
            _logger.LogTrace("WebhookLogic created");
        }
        public WebhookResponse ListParcelWebhooks(string strackingId)
        {
            throw new NotImplementedException();
        }

        public WebhookResponse SubscribeParcelWebhook(string strackingId, string url)
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeParcelWebhook(long id)
        {
            throw new NotImplementedException();
        }
    }
}