using Microsoft.Extensions.Logging;
using System;

namespace AuthorizationService.Extensions
{
    public static class LoggerExtensions
    {
        public static void Debug(this ILogger logger, string message)
        {
            logger.LogDebug(message);
        }

        public static void Info(this ILogger logger, string message)
        {
            logger.LogInformation(message);
        }
        public static void Warn(this ILogger logger, string message)
        {
            logger.LogWarning(message);
        }

        public static void Error(this ILogger logger, string message)
        {
            logger.LogError(message);
        }

        public static void Error(this ILogger logger, Exception ex)
        {
            logger.LogError(ex.ToString());
        }
    }
}
