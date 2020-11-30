using System.Collections.Generic;
using KochWermann.SKS.Package.DataAccess.Entities;
using KochWermann.SKS.Package.DataAccess.Interfaces;

namespace KochWermann.SKS.Package.DataAccess.Sql
{
    public class SqlWebhookRepository : IWebhookRepository
    {
        public long Create(WebhookResponse webhookResponse)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(long Id)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(IEnumerable<WebhookResponse> hooks)
        {
            throw new System.NotImplementedException();
        }

        public WebhookResponse GetById(long Id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<WebhookResponse> GetByTrackingId(string trackingId)
        {
            throw new System.NotImplementedException();
        }
    }
}
