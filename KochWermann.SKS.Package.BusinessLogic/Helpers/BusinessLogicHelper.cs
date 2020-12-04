using System;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace KochWermann.SKS.Package.BusinessLogic.Helpers
{
    public static class BusinessLogicHelper
    {
        public static void Validate<T>(T instanceToValidate, AbstractValidator<T> validator, ILogger logger) 
        {
            ValidationResult validationResult;

            try
            {
                validationResult = validator.Validate(instanceToValidate);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in validation: {ex}");
                throw new ValidationException($"Error in validation: {ex}");
            }

            if (!validationResult.IsValid)
            {
                var ex = new ValidationException(validationResult.Errors);
                logger.LogError($"ValidationResult is not valid: {ex}");
                throw ex;
            }     
        }
    }
}