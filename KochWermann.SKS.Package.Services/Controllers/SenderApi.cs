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
    public class SenderApiController : ControllerBase
    { 
        private readonly IMapper _mapper;
        private ITrackingLogic _trackingLogic;

        /// <summary>
        /// 
        /// </summary>
        public SenderApiController(IMapper mapper, ITrackingLogic trackingLogic)
        {
            _mapper = mapper;
            _trackingLogic = trackingLogic;
        }

        /// <summary>
        /// Submit a new parcel to the logistics service. 
        /// </summary>
        /// <param name="body"></param>
        /// <response code="200">Successfully submitted the new parcel</response>
        /// <response code="400">The operation failed due to an error.</response>
        [HttpPost]
        [Route("/parcel")]
        [ValidateModelState]
        [SwaggerOperation("SubmitParcel")]
        [SwaggerResponse(statusCode: 200, type: typeof(DTOs.NewParcelInfo), description: "Successfully submitted the new parcel")]
        [SwaggerResponse(statusCode: 400, type: typeof(DTOs.Error), description: "The operation failed due to an error.")]
        public virtual IActionResult SubmitParcel([FromBody]DTOs.Parcel body)
        {
            if (body != null)
            {
                var blParcel = _mapper.Map<BusinessLogic.Entities.Parcel>(body);
                var blSubmitedParcel = _trackingLogic.SubmitParcel(blParcel);
                var serviceNewParcelInfo = _mapper.Map<DTOs.NewParcelInfo>(blSubmitedParcel);
                return Ok(serviceNewParcelInfo);
            }

            return BadRequest(new DTOs.Error{ ErrorMessage = "body is null" });
        }
    }
}
