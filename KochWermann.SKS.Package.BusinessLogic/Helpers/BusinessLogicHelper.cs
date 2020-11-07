using System;
using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace KochWermann.SKS.Package.BusinessLogic.Helpers
{
    public static class BusinessLogicHelper
    {
        public static void Validate<T>(T instanceToValidate, AbstractValidator<T> validator, ILogger logger)
        {
            var validationResult = validator.Validate(instanceToValidate);
            if (!validationResult.IsValid)
            {
                var ex = new ValidationException(validationResult.Errors);
                logger.LogError(ex.ToString());
                throw ex;
            }            
        }

        public static BL_Exception ExceptionHandler(string method, Exception ex, ILogger logger)
        {
            logger.LogError(ex.ToString());
            return new BL_Exception(method, ex);
        }

        public static BL_NotFound_Exception NotFound_ExceptionHandler(string method, Exception ex, ILogger logger)
        {
            logger.LogError(ex.ToString());
            return new BL_NotFound_Exception(method, ex);
        }
    }
}