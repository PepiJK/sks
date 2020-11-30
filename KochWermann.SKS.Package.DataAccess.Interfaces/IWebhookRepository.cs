using KochWermann.SKS.Package.DataAccess.Entities;
using System.Collections.Generic;

namespace KochWermann.SKS.Package.DataAccess.Interfaces
{
    public interface IWebhookRepository
    {
        long Create (WebhookResponse webhookResponse);

        void Delete (long Id);

        void Delete (IEnumerable<WebhookResponse> hooks);

        IEnumerable<WebhookResponse> GetByTrackingId (string trackingId);

        WebhookResponse GetById (long Id);
    }
}