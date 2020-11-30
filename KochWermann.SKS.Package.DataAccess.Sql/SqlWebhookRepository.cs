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
            catch (SqlException ex)
            {
                _logger.LogError($"Error in Create WebhookResponse {ex}");
                throw new DALException("Error in Create WebhookResponse", ex);
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
                _context.Remove(_context.Webhooks.Single(x => x.Id == id));
                _context.SaveChanges();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Error in Delete Webhook {ex}");
                throw new DALException("Error in Delete Webhook", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Delete Webhook {ex}");
                throw new DALException("Error in Delete Webhook", ex);
            }
        }

        public void Delete(IEnumerable<WebhookResponse> hooks)
        {
            _context.RemoveRange(_context.Webhooks.Where(p => hooks.Any(x => x.Id == p.Id)));
            _context.SaveChanges();
        }

        public WebhookResponse GetById(long id)
        {
            try
            {
                return _context.Webhooks.Single(x => x.Id == id);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.ToString());
                throw new DALNotFoundException("Error in Getting WebhookResponse", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Getting WebhookResponse {ex}");
                throw new DALException("Error in Getting WebhookResponse", ex);
            }
        }

        public IEnumerable<WebhookResponse> GetByTrackingId(string trackingId)
        {
            return _context.Webhooks.Where(hook => hook.TrackingId == trackingId);
        }
    }
}
