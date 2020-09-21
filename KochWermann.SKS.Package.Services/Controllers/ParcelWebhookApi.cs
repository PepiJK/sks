/*
 * Parcel Logistics Service
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.20.0
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
        /// 
        /// </summary>
        /// <remarks>Gets all registered subscriptions for the parcel webhook.</remarks>
        /// <param name="trackingId"></param>
        /// <response code="200">List of webooks for the &#x60;trackingId&#x60;</response>
        /// <response code="404">No parcel found with that tracking ID.</response>
        [HttpGet]
        [Route("/api/parcel/{trackingId}/webhooks")]
        [ValidateModelState]
        [SwaggerOperation("ApiParcelByTrackingIdWebhooksGet")]
        [SwaggerResponse(statusCode: 200, type: typeof(WebhookResponses), description: "List of webooks for the &#x60;trackingId&#x60;")]
        public virtual IActionResult ApiParcelByTrackingIdWebhooksGet([FromRoute][Required][RegularExpression("/^[A-Z0-9]{9}$/")]string trackingId)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(WebhookResponses));

            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404);
            string exampleJson = null;
            exampleJson = "[ {\n  \"created_at\" : \"2000-01-23T04:56:07.000+00:00\",\n  \"id\" : 0,\n  \"url\" : \"url\",\n  \"trackingId\" : \"trackingId\"\n}, {\n  \"created_at\" : \"2000-01-23T04:56:07.000+00:00\",\n  \"id\" : 0,\n  \"url\" : \"url\",\n  \"trackingId\" : \"trackingId\"\n} ]";
            
                        var example = exampleJson != null
                        ? JsonConvert.DeserializeObject<WebhookResponses>(exampleJson)
                        : default(WebhookResponses);            //TODO: Change the data returned
            return new ObjectResult(example);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Subscribes to a webhook notification for the specific parcel.</remarks>
        /// <param name="trackingId"></param>
        /// <param name="url"></param>
        /// <response code="200">Successful response</response>
        /// <response code="404">No parcel found with that tracking ID.</response>
        [HttpPost]
        [Route("/api/parcel/{trackingId}/webhooks")]
        [ValidateModelState]
        [SwaggerOperation("ApiParcelByTrackingIdWebhooksPost")]
        [SwaggerResponse(statusCode: 200, type: typeof(WebhookResponse), description: "Successful response")]
        public virtual IActionResult ApiParcelByTrackingIdWebhooksPost([FromRoute][Required][RegularExpression("/^[A-Z0-9]{9}$/")]string trackingId, [FromQuery][Required()]string url)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(WebhookResponse));

            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404);
            string exampleJson = null;
            exampleJson = "{\n  \"created_at\" : \"2000-01-23T04:56:07.000+00:00\",\n  \"id\" : 0,\n  \"url\" : \"url\",\n  \"trackingId\" : \"trackingId\"\n}";
            
                        var example = exampleJson != null
                        ? JsonConvert.DeserializeObject<WebhookResponse>(exampleJson)
                        : default(WebhookResponse);            //TODO: Change the data returned
            return new ObjectResult(example);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Removes an existing webhook subscription.</remarks>
        /// <param name="id"></param>
        /// <response code="200">Success</response>
        /// <response code="404">Subscription does not exist.</response>
        [HttpDelete]
        [Route("/api/parcel/webhooks/{id}")]
        [ValidateModelState]
        [SwaggerOperation("ApiParcelWebhooksByIdDelete")]
        public virtual IActionResult ApiParcelWebhooksByIdDelete([FromRoute][Required]long? id)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);

            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404);

            throw new NotImplementedException();
        }
    }
}
