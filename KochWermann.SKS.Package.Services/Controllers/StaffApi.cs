/*
 * Parcel Logistics Service
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.20.1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using KochWermann.SKS.Package.Services.Attributes;
using AutoMapper;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace KochWermann.SKS.Package.Services.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class StaffApiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITrackingLogic _trackingLogic;
        private readonly ILogger<StaffApiController> _logger;


        /// <summary>
        /// 
        /// </summary>
        public StaffApiController(IMapper mapper, ITrackingLogic trackingLogic, ILogger<StaffApiController> logger)
        {
            _mapper = mapper;
            _trackingLogic = trackingLogic;
            _logger = logger;
            _logger.LogTrace("StaffApiController created");
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
        [SwaggerResponse(statusCode: 200, type: typeof(string), description: "Successfully reported hop.")]
        [SwaggerResponse(statusCode: 400, type: typeof(DTOs.Error), description: "The operation failed due to an error.")]
        [SwaggerResponse(statusCode: 404, type: typeof(DTOs.Error), description: "Parcel does not exist with this tracking ID.")]
        public virtual IActionResult ReportParcelDelivery([FromRoute][Required][RegularExpression("^[A-Z0-9]{9}$")] string trackingId)
        {
            try
            {
                _logger.LogTrace($"ReportParcelDelivery: trackingId: {trackingId}");

                if (string.IsNullOrWhiteSpace(trackingId))
                {
                    _logger.LogError("TrackingId is null or white space");
                    return BadRequest(new DTOs.Error { ErrorMessage = "TrackingId is null or white space" });
                }

                _trackingLogic.ReportParcelDelivery(trackingId);

                return Ok("Successfull delivery");
            }
            catch (BusinessLogic.Entities.BLNotFoundException ex)
            {
                _logger.LogError($"No Parcel exist with this trackingId {ex}");
                return NotFound(new DTOs.Error { ErrorMessage = "No Parcel exist with this trackingId" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"The operation failed due to an error {ex}");
                return BadRequest(new DTOs.Error { ErrorMessage = "The operation failed due to an error" });
            }
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
        [SwaggerResponse(statusCode: 200, type: typeof(string), description: "Successfully reported hop.")]
        [SwaggerResponse(statusCode: 400, type: typeof(DTOs.Error), description: "The operation failed due to an error.")]
        [SwaggerResponse(statusCode: 404, type: typeof(DTOs.Error), description: "No parcel exists with this tracking ID.")]
        public virtual IActionResult ReportParcelHop([FromRoute][Required][RegularExpression("^[A-Z0-9]{9}$")] string trackingId, [FromRoute][Required][RegularExpression("^[A-Z]{4}\\d{1,4}$")] string code)
        {
            try
            {
                _logger.LogTrace($"ReportParcelHop: trackingId: {trackingId} and code: {code}");

                if (string.IsNullOrWhiteSpace(trackingId))
                {
                    _logger.LogError("TrackingId is null or white space");
                    return BadRequest(new DTOs.Error { ErrorMessage = "TrackingId is null or white space" });
                }

                if (string.IsNullOrWhiteSpace(code))
                {
                    _logger.LogError("Code is null or white space");
                    return BadRequest(new DTOs.Error { ErrorMessage = "Code is null or white space" });
                }

                _trackingLogic.ReportParcelHop(trackingId, code);

                return Ok("Successfully reported hop");
            }
            catch (BusinessLogic.Entities.BLNotFoundException ex)
            {
                _logger.LogError($"TrackingId or code could not be found {ex}");
                return NotFound(new DTOs.Error { ErrorMessage = "TrackingId or code could not be found" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"The operation failed due to an error {ex}");
                return BadRequest(new DTOs.Error { ErrorMessage = "The operation failed due to an error" });
            }
        }
    }
}
