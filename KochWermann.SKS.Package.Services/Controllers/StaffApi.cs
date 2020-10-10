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
    public class StaffApiController : ControllerBase
    { 
        private readonly IMapper _mapper;
        private ITrackingLogic _trackingLogic;
        
        /// <summary>
        /// 
        /// </summary>
        public StaffApiController(IMapper mapper, ITrackingLogic trackingLogic)
        {
            _mapper = mapper;
            _trackingLogic = trackingLogic;
        }

        /// <summary>
        /// Report that a Parcel has been delivered at it&#x27;s final destination address. 
        /// </summary>
        /// <param name="trackingId">The tracking ID of the parcel. E.g. PYJRB4HZ6 </param>
        /// <response code="200">Successfully reported hop.</response>
        /// <response code="400">The operation failed due to an error.</response>
        /// <response code="404">Parcel does not exist with this tracking ID. </response>
        [HttpPost]
        [Route("/parcel/{trackingId}/reportDelivery/")]
        [ValidateModelState]
        [SwaggerOperation("ReportParcelDelivery")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "The operation failed due to an error.")]
        public virtual IActionResult ReportParcelDelivery([FromRoute][Required][RegularExpression("/^[A-Z0-9]{9}$/")]string trackingId)
        {
            //TODO: is Regex is wrong?, ^[A-Z0-9]{9}$ matches PYJRB4HZ6
            if (!string.IsNullOrWhiteSpace(trackingId))
            {
                if (trackingId == "ERROR1234")
                    return BadRequest(new Error{ ErrorMessage = "trackingId is ERROR1234" });

                _trackingLogic.ReportParcelDelivery(trackingId);
                return Ok();
            }

            return NotFound();
        }

        /// <summary>
        /// Report that a Parcel has arrived at a certain hop either Warehouse or Truck. 
        /// </summary>
        /// <param name="trackingId">The tracking ID of the parcel. E.g. PYJRB4HZ6 </param>
        /// <param name="code">The Code of the hop (Warehouse or Truck).</param>
        /// <response code="200">Successfully reported hop.</response>
        /// <response code="400">The operation failed due to an error.</response>
        /// <response code="404">Parcel does not exist with this tracking ID or hop with code not found. </response>
        [HttpPost]
        [Route("/parcel/{trackingId}/reportHop/{code}")]
        [ValidateModelState]
        [SwaggerOperation("ReportParcelHop")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "The operation failed due to an error.")]
        public virtual IActionResult ReportParcelHop([FromRoute][Required][RegularExpression("/^[A-Z0-9]{9}$/")]string trackingId, [FromRoute][Required][RegularExpression("/^[A-Z]{4}\\d{1,4}$/")]string code)
        {
            //TODO: is Regex is wrong?, ^[A-Z0-9]{9}$ matches PYJRB4HZ6
            if (!string.IsNullOrWhiteSpace(trackingId) && !string.IsNullOrWhiteSpace(code))
            {
                if (trackingId == "ERROR1234" || code == "ERRO\\d")
                    return BadRequest(new Error{ ErrorMessage = "trackingId is ERROR1234 or code is ERRO\\d" });
                
                _trackingLogic.ReportParcelHop(trackingId, code);
                return Ok();
            }

            return NotFound();
        }
    }
}
