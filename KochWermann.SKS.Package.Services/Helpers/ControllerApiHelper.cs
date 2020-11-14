using System;
using Microsoft.Extensions.Logging;

namespace KochWermann.SKS.Package.Services.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class ControllerApiHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static DTOs.Error CreateErrorDTO(string message, ILogger logger, Exception ex = null)
        {
            if (ex != null)
            {
                logger.LogError(ex.ToString());
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    message += " " + ex.ToString();
                }
            }
            return new DTOs.Error{ErrorMessage = message};
        }
    }
}