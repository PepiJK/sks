/*
 * Parcel Logistics Service
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.20.1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using KochWermann.SKS.Package.Services.Attributes;

using Microsoft.AspNetCore.Authorization;
using KochWermann.SKS.Package.Services.DTOs;

namespace KochWermann.SKS.Package.Services.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class ParcelWebhookApiController : ControllerBase
    { 
        /// <summary>
        /// Get all registered subscriptions for the parcel webhook.
        /// </summary>
        /// <param name="trackingId"></param>
        /// <response code="200">List of webooks for the &#x60;trackingId&#x60;</response>
        /// <response code="404">No parcel found with that tracking ID.</response>
        [HttpGet]
        [Route("/parcel/{trackingId}/webhooks")]
        [ValidateModelState]
        [SwaggerOperation("ListParcelWebhooks")]
        [SwaggerResponse(statusCode: 200, type: typeof(DTOs.WebhookResponses), description: "List of webooks for the &#x60;trackingId&#x60;")]
        [SwaggerResponse(statusCode: 404, type: typeof(DTOs.Error), description: "No parcel found with that tracking ID.")]
        public virtual IActionResult ListParcelWebhooks([FromRoute][Required][RegularExpression("^[A-Z0-9]{9}$")]string trackingId)
        { 
            if (!string.IsNullOrWhiteSpace(trackingId))
                return Ok(default(WebhookResponses));
            
            return NotFound();
        }

        /// <summary>
        /// Subscribe to a webhook notification for the specific parcel.
        /// </summary>
        /// <param name="trackingId"></param>
        /// <param name="url"></param>
        /// <response code="200">Successful response</response>
        /// <response code="404">No parcel found with that tracking ID.</response>
        [HttpPost]
        [Route("/parcel/{trackingId}/webhooks")]
        [ValidateModelState]
        [SwaggerOperation("SubscribeParcelWebhook")]
        [SwaggerResponse(statusCode: 200, type: typeof(DTOs.WebhookResponse), description: "Successful response")]
        [SwaggerResponse(statusCode: 404, type: typeof(DTOs.Error), description: "No parcel found with that tracking ID.")]
        public virtual IActionResult SubscribeParcelWebhook([FromRoute][Required][RegularExpression("^[A-Z0-9]{9}$")]string trackingId, [FromQuery][Required()]string url)
        { 
            if (!string.IsNullOrWhiteSpace(trackingId) && !string.IsNullOrWhiteSpace(url))
                return Ok(default(DTOs.WebhookResponse));
            
            return NotFound();
        }

        /// <summary>
        /// Remove an existing webhook subscription.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Success</response>
        /// <response code="404">Subscription does not exist.</response>
        [HttpDelete]
        [Route("/parcel/webhooks/{id}")]
        [ValidateModelState]
        [SwaggerOperation("UnsubscribeParcelWebhook")]
        [SwaggerResponse(statusCode: 200, type: typeof(DTOs.WebhookResponse), description: "Successful response")]
        [SwaggerResponse(statusCode: 404, type: typeof(DTOs.Error), description: "Subscription does not exist.")]
        public virtual IActionResult UnsubscribeParcelWebhook([FromRoute][Required]long? id)
        { 
            if (id != null && id > 0)
                return Ok();

            return NotFound();
        }
    }
}
