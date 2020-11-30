using Microsoft.Extensions.Logging;
using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.ServiceAgents.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace KochWermann.SKS.Package.ServiceAgents
{
    public class RestWebhookAgent : IWebhookAgent
    {
        private readonly ILogger<RestWebhookAgent> _logger;

        public RestWebhookAgent (ILogger<RestWebhookAgent> logger)
        {
            _logger = logger;
        }

        public void Notify (IEnumerable<WebhookResponse> subscriber, WebhookMessage message)
        {
            try
            {
                foreach (var sub in subscriber)
                {
                    _logger.LogInformation($"notifying subscriber for parcel {sub.TrackingId} on \"{sub.Url}\"");
                    _logger.LogInformation($"WebhookMessage: {message.TrackingId ?? "no trackingId" }, {message.State}");
                    if (message.VisitedHops != null) _logger.LogInformation($"{message.VisitedHops.Count}");
                    if (message.FutureHops != null) _logger.LogInformation($"{message.FutureHops.Count}");
                    using var client = new HttpClient();
                    client.PostAsJsonAsync(sub.Url, message);
                }
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.ToString());
                throw;
            }
        }
    }
}