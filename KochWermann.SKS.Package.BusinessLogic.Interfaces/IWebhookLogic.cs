using KochWermann.SKS.Package.BusinessLogic.Entities;
using System.Collections.Generic;

namespace KochWermann.SKS.Package.BusinessLogic.Interfaces
{
    public interface IWebhookLogic
    {
        /// <summary>
        /// Get all registered subscriptions for the parcel webhook.
        /// </summary>
        IEnumerable<WebhookResponse> ListParcelWebhooks(string strackingId);

        /// <summary>
        /// Subscribe to a webhook notification for the specific parcel.
        /// </summary>
        WebhookResponse SubscribeParcelWebhook(string strackingId, string url);

        /// <summary>
        /// Remove an exisiting webhook subscription.
        /// </summary>
        void UnsubscribeParcelWebhook(long id);
    }
}