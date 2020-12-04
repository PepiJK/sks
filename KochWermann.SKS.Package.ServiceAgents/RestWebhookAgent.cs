using Microsoft.Extensions.Logging;
using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.ServiceAgents.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using KochWermann.SKS.Package.ServiceAgents.Exceptions;

namespace KochWermann.SKS.Package.ServiceAgents
{
    public class RestWebhookAgent : IWebhookAgent
    {
        private readonly ILogger<RestWebhookAgent> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public RestWebhookAgent (ILogger<RestWebhookAgent> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public void Notify (IEnumerable<WebhookResponse> subscriber, WebhookMessage message)
        {
            try
            {
                foreach (var sub in subscriber)
                {
                    _logger.LogInformation($"notifying subscriber for parcel {sub.TrackingId} on \"{sub.Url}\"");
                    _logger.LogInformation($"WebhookMessage: {message.TrackingId ?? "no trackingId" }, {message.State}");

                    if (message.VisitedHops != null) _logger.LogInformation($"Visited Hops: {message.VisitedHops.Count}");
                    if (message.FutureHops != null) _logger.LogInformation($"Future Hops: {message.FutureHops.Count}");


                    var request = new HttpRequestMessage(HttpMethod.Post, sub.Url);
                    var serilizedMessage = JsonConvert.SerializeObject(message, Formatting.None, new JsonSerializerSettings { 
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                    request.Content = new StringContent(serilizedMessage, Encoding.UTF8, "application/json");

                    var client = _clientFactory.CreateClient("webhooks");
                    var response = client.SendAsync(request).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError($"Webhook request error, status {response.StatusCode}");
                        throw new ServiceAgentRequestException($"Webhook request error, status {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Notify Webhook Subscribers {ex}");
                throw new ServiceAgentException("Error in Notify Webhook Subscribers", ex);
            }
        }
    }
}