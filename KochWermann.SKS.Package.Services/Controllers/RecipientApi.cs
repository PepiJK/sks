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
    public class RecipientApiController : ControllerBase
    { 
        /// <summary>
        /// Find the latest state of a parcel by its tracking ID. 
        /// </summary>
        /// <param name="trackingId">The tracking ID of the parcel. E.g. PYJRB4HZ6 </param>
        /// <response code="200">Parcel exists, here&#x27;s the tracking information.</response>
        /// <response code="400">The operation failed due to an error.</response>
        /// <response code="404">Parcel does not exist with this tracking ID.</response>
        [HttpGet]
        [Route("/parcel/{trackingId}")]
        [ValidateModelState]
        [SwaggerOperation("TrackParcel")]
        [SwaggerResponse(statusCode: 200, type: typeof(TrackingInformation), description: "Parcel exists, here&#x27;s the tracking information.")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "The operation failed due to an error.")]
        public virtual IActionResult TrackParcel([FromRoute][Required][RegularExpression("/^[A-Z0-9]{9}$/")]string trackingId)
        { 
            if (!string.IsNullOrWhiteSpace(trackingId))
            {
                if (trackingId == "ERROR1234")
                    return StatusCode(400, default(Error));

                return StatusCode(200, default(TrackingInformation));
            }
                
            return StatusCode(404); 
        }
    }
}