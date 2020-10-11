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
using AutoMapper;
using KochWermann.SKS.Package.BusinessLogic;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;

namespace KochWermann.SKS.Package.Services.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class LogisticsPartnerApiController : ControllerBase
    { 
        private readonly IMapper _mapper;
        private ITrackingLogic _trackingLogic;
        
        /// <summary>
        /// 
        /// </summary>
        public LogisticsPartnerApiController(IMapper mapper, ITrackingLogic trackingLogic)
        {
            _mapper = mapper;
            _trackingLogic = trackingLogic;
        }

        /// <summary>
        /// Transfer an existing parcel into the system from the service of a logistics partner. 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="trackingId">The tracking ID of the parcel. E.g. PYJRB4HZ6 </param>
        /// <response code="200">Successfully transitioned the parcel</response>
        /// <response code="400">The operation failed due to an error.</response>
        [HttpPost]
        [Route("/parcel/{trackingId}")]
        [ValidateModelState]
        [SwaggerOperation("TransitionParcel")]
        [SwaggerResponse(statusCode: 200, type: typeof(NewParcelInfo), description: "Successfully transitioned the parcel")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "The operation failed due to an error.")]
        public virtual IActionResult TransitionParcel([FromBody]Parcel body, [FromRoute][Required][RegularExpression("/^[A-Z0-9]{9}$/")]string trackingId)
        {
            //TODO: is Regex is wrong?, ^[A-Z0-9]{9}$ matches PYJRB4HZ6
            if (body != null && !string.IsNullOrWhiteSpace(trackingId))
            {
                var blParcel = _mapper.Map<BusinessLogic.Entities.Parcel>(body);
                var blTransitionedParcel = _trackingLogic.TransitionParcel(blParcel, trackingId);
                var serviceNewParcelInfo = _mapper.Map<DTOs.NewParcelInfo>(blTransitionedParcel);
                return Ok(serviceNewParcelInfo);
            }
             
            return BadRequest(new Error{ ErrorMessage = "body or trackingId is null or whitespace" });
        }
    }
}

