using System;
using System.Collections.Generic;
using AutoMapper;
using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.BusinessLogic.Helpers;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using KochWermann.SKS.Package.BusinessLogic.Validators;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace KochWermann.SKS.Package.BusinessLogic
{
    public class WebhookLogic : IWebhookLogic
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IWebhookRepository _webhookRepository;
        private readonly IParcelRepository _parcelRepository;
        private readonly TrackingIdValidator _trackingIdValidator = new TrackingIdValidator();
        private readonly UrlValidator _urlValidator = new UrlValidator();

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

                var hooks = _webhookRepository.GetByTrackingId(trackingId);

                return _mapper.Map<IEnumerable<WebhookResponse>>(hooks);
            }
            catch (DataAccess.Entities.DALNotFoundException ex)
            {
                _logger.LogError($"Could not find trackinId {trackingId} {ex}");
                throw new BLNotFoundException($"Could not find trackinId {trackingId}", ex);
            }
            catch (ValidationException ex)
            {
                _logger.LogError($"Validation error in trackingId {ex}");
                throw new BLValidationException("Validation error in trackingId", ex);            
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ListParcelWebhooks {ex}");
                throw new BLException("Error in ListParcelWebhooks", ex);
            }
        }

        public WebhookResponse SubscribeParcelWebhook(string trackingId, string url)
        {
            try
            {
                BusinessLogicHelper.Validate<string>(trackingId, _trackingIdValidator, _logger);
                BusinessLogicHelper.Validate<string>(url, _urlValidator, _logger);

                if (!_parcelRepository.ContainsTrackingId(trackingId))
                {
                    throw new DataAccess.Entities.DALNotFoundException($"No parcel with trackingId {trackingId} exists");
                }

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
                _logger.LogError($"Could not find newly created webhook or parcel with trackingId {trackingId} {ex}");
                throw new BLNotFoundException($"Could not find newly created webhook or parcel with trackingId {trackingId}", ex);
            }
            catch (ValidationException ex)
            {
                _logger.LogError($"Validation error in trackingId {ex}");
                throw new BLValidationException("Validation error in trackingId", ex);            
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in SubscribeParcelWebhook {ex}");
                throw new BLException("Error in SubscribeParcelWebhook", ex);
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
                _logger.LogError($"Could not find exactly one webhook with id {id} {ex}");
                throw new BLNotFoundException($"Could not find exactly one webhook with id {id}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UnsubscribeParcelWebhook {ex}");
                throw new BLException("Error in UnsubscribeParcelWebhook", ex);
            }
        }
    }
}