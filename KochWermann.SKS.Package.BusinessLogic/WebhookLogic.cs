using System;
using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;

namespace KochWermann.SKS.Package.BusinessLogic
{
    public class WebhookLogic : IWebhookLogic
    {
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