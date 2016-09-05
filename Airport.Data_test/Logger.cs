using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.IO;

namespace Airport.Gate.Data.Log
{
    public class Logger
    {
        public ILog log = null;
        public Logger()
        {
            log = log4net.LogManager.GetLogger("Logger");
        }

        public Logger(Type type)
        {
            log = log4net.LogManager.GetLogger(type);
        }

        private static Logger logger = new Logger();


        public static void Dispose()
        {
            if (logger.log != null)
            {
                logger.log.Logger.Repository.Shutdown();
            }
        }

        public static void Error(Exception e)
        {
            logger.log.Error(e.Message, e);
        }
        
        public static void Error(String message)
        {
            logger.log.Error(message);
        }
        
        public static void Debug(String message)
        {
            logger.log.Debug(message);
        }
        
        public static void Info(String message)
        {
            logger.log.Info(message);
        }
    }
}
