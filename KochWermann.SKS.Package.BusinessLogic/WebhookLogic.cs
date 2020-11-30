using System;
using System.Collections.Generic;
using AutoMapper;
using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.BusinessLogic.Helpers;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using KochWermann.SKS.Package.BusinessLogic.Validators;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;

namespace KochWermann.SKS.Package.BusinessLogic
{
    public class WebhookLogic : IWebhookLogic
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IWebhookRepository _webhookRepository;
        private readonly IParcelRepository _parcelRepository;
        private readonly TrackingIdValidator _trackingIdValidator = new TrackingIdValidator();

        public WebhookLogic(IWebhookRepository webhookRepository, IParcelRepository parcelRepository, IMapper mapper, ILogger<WebhookLogic> logger)
        {
            _mapper = mapper;
            _logger = logger;
            _webhookRepository = webhookRepository;
            _parcelRepository = parcelRepository;
            _logger.LogTrace("WebhookLogic created");
        }
        public IEnumerable<WebhookResponse> ListParcelWebhooks(string trackingId)
        {
            try
            {
                BusinessLogicHelper.Validate<string>(trackingId, _trackingIdValidator, _logger);

                _parcelRepository.GetParcelByTrackingId(trackingId);

                var hooks = _webhookRepository.GetByTrackingId(trackingId);

                return _mapper.Map<IEnumerable<WebhookResponse>>(hooks);
            }
            catch (DataAccess.Entities.DALNotFoundException ex)
            {
                _logger.LogError(ex.ToString());
                throw new BLNotFoundException($"Could not find trackinId {trackingId}", ex);
            }
            catch (DataAccess.Entities.DALException ex)
            {
                _logger.LogError($"Error: {ex}");
                throw new BLException("Error: ", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error {ex}");
                throw new BLException("Error: ", ex);
            }
        }

        public WebhookResponse SubscribeParcelWebhook(string trackingId, string url)
        {
            try
            {
                BusinessLogicHelper.Validate<string>(trackingId, _trackingIdValidator, _logger);

                _parcelRepository.GetParcelByTrackingId(trackingId);

                var webhook = new DataAccess.Entities.WebhookResponse()
                {
                    TrackingId = trackingId,
                    CreatedAt = DateTime.Now,
                    Url = url
                };
                var id = _webhookRepository.Create(webhook);
                var hook = _webhookRepository.GetById(id);
                return _mapper.Map<WebhookResponse>(hook);
            }
            catch (DataAccess.Entities.DALNotFoundException ex)
            {
                _logger.LogError(ex.ToString());
                throw new BLNotFoundException($"Error: ", ex);
            }
            catch (DataAccess.Entities.DALException ex)
            {
                _logger.LogError($"Error: {ex}");
                throw new BLException("Error: ", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex}");
                throw new BLException("Error: ", ex);
            }
        }

        public void UnsubscribeParcelWebhook(long id)
        {
            try
            {
                _webhookRepository.Delete(id);
            }
            catch (DataAccess.Entities.DALNotFoundException ex)
            {
                _logger.LogError(ex.ToString());
                throw new BLNotFoundException($"Error: ", ex);
            }
            catch (DataAccess.Entities.DALException ex)
            {
                _logger.LogError($"Error: {ex}");
                throw new BLException("Error: ", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex}");
                throw new BLException("Error: ", ex);
            }
        }
    }
}