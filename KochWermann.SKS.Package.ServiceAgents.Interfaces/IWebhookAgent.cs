using System.Collections.Generic;
using KochWermann.SKS.Package.BusinessLogic.Entities;

namespace KochWermann.SKS.Package.ServiceAgents.Interfaces
{
    public interface IWebhookAgent
    {
        void Notify (IEnumerable<WebhookResponse> subscriber, WebhookMessage message);
    }
}