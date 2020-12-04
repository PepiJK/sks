using System.Collections.Generic;
using KochWermann.SKS.Package.DataAccess.Entities;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;
using Microsoft.Data.SqlClient;

namespace KochWermann.SKS.Package.DataAccess.Sql
{
    public class SqlWebhookRepository : IWebhookRepository
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<SqlWebhookRepository> _logger;

        public SqlWebhookRepository(DatabaseContext context, ILogger<SqlWebhookRepository> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogTrace("SqlWebhookRepository created ");
        }

        public long Create(WebhookResponse response)
        {
            try
            {
                _context.Webhooks.Add(response);
                _context.SaveChanges();
                
                return response.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Create WebhookResponse {ex}");
                throw new DALException("Error in Create WebhookResponse", ex);
            }
        }

        public void Delete(long id)
        {
            try
            {
                var webhook = GetById(id);

                _context.Webhooks.Remove(webhook);
                _context.SaveChanges();
            }
            catch (DALNotFoundException ex)
            {
                _logger.LogError($"Could not find exactly one webhook with id {id} {ex}");
                throw new DALException($"Could not find exactly one parcel with id {id}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Delete Webhook {ex}");
                throw new DALException("Error in Delete Webhook", ex);
            }
        }

        public void Delete(IEnumerable<WebhookResponse> hooks)
        {
            try
            {
                _context.Webhooks.RemoveRange(hooks);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Delete Webhooks {ex}");
                throw new DALException("Error in Delete Webhooks", ex);
            }
        }

        public WebhookResponse GetById(long id)
        {
            try
            {
                return _context.Webhooks.Single(x => x.Id == id);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Could not find exactly one webhook with id {id} {ex}");
                throw new DALNotFoundException($"Could not find exactly one webhook with id {id}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetById WebhookResponse {ex}");
                throw new DALException("Error in GetById WebhookResponse", ex);
            }
        }

        public IEnumerable<WebhookResponse> GetByTrackingId(string trackingId)
        {
            try
            {
                return _context.Webhooks.Where(hook => hook.TrackingId == trackingId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetByTrackingId WebhookResponse {ex}");
                throw new DALException("Error in GetByTrackingId WebhookResponse", ex);
            }
        }
    }
}
