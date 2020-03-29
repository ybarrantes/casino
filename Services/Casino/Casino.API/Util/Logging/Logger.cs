using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casino.API.Util.Logging
{
    public class Logger
    {
        protected static ILogger logger = null;

        protected static ILogger Log
        {
            get
            {
                if (logger == null)
                    logger = Config.ApiConfig.Singleton.LoggerFactory.CreateLogger<Logger>();

                if (logger == null) throw new NullReferenceException();

                return logger;
            }
        }

        public static void Error(string Message, params object[] args)
        {
            Log.LogError(Message, args);
        }

        public static void Error(Exception exception, string Message, params object[] args)
        {
            Log.LogError(exception, Message, args);
        }

        public static void Info(string Message, params object[] args)
        {
            Log.LogInformation(Message, args);
        }

        public static void Info(Exception exception, string Message, params object[] args)
        {
            Log.LogInformation(exception, Message, args);
        }

        public static void Debug(string Message, params object[] args)
        {
            Log.LogDebug(Message, args);
        }

        public static void Debug(Exception exception, string Message, params object[] args)
        {
            Log.LogDebug(exception, Message, args);
        }

        public static void Warning(string Message, params object[] args)
        {
            Log.LogWarning(Message, args);
        }

        public static void Warning(Exception exception, string Message, params object[] args)
        {
            Log.LogWarning(exception, Message, args);
        }

        public static void Critical(string Message, params object[] args)
        {
            Log.LogCritical(Message, args);
        }

        public static void Critical(Exception exception, string Message, params object[] args)
        {
            Log.LogCritical(exception, Message, args);
        }

        public static void Trace(string Message, params object[] args)
        {
            Log.LogTrace(Message, args);
        }

        public static void Trace(Exception exception, string Message, params object[] args)
        {
            Log.LogTrace(exception, Message, args);
        }
    }
}
