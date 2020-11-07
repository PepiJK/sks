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
    public class SenderApiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITrackingLogic _trackingLogic;
        private readonly ILogger<SenderApiController> _logger;


        /// <summary>
        /// 
        /// </summary>
        public SenderApiController(IMapper mapper, ITrackingLogic trackingLogic, ILogger<SenderApiController> logger)
        {
            _mapper = mapper;
            _trackingLogic = trackingLogic;
            _logger = logger;
            _logger.LogTrace("SenderApiController created");
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
        public virtual IActionResult SubmitParcel([FromBody] DTOs.Parcel body)
        {
            try
            {
                _logger.LogTrace($"SubmitParcel");
                if (body == null)
                    return BadRequest(new DTOs.Error { ErrorMessage = "body is null" });

                var blParcel = _mapper.Map<BusinessLogic.Entities.Parcel>(body);
                var blSubmitedParcel = _trackingLogic.SubmitParcel(blParcel);
                var serviceNewParcelInfo = _mapper.Map<DTOs.NewParcelInfo>(blSubmitedParcel);
                return Ok(serviceNewParcelInfo);
            }
            catch (BusinessLogic.Entities.BL_Exception ex)
            {
                return ExceptionHandler("Error.", ex);
            }
            catch (Exception ex)
            {
                return ExceptionHandler("Error.", ex);
            }
        }

        private IActionResult ExceptionHandler(string message, Exception ex = null)
        {
            if (ex != null)
            {
                _logger.LogError(ex.ToString());
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    message += "\n" + ex.Message + "\n" + ex.StackTrace;
                    if (ex.InnerException.InnerException != null)
                    {
                        message += "\n" + ex.InnerException.InnerException.Message + "\n" + ex.InnerException.InnerException.StackTrace;
                    }
                }
            }
            return BadRequest(new DTOs.Error{ErrorMessage = message});
        }
    }
}
