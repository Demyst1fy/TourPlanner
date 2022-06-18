using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.BusinessLayer.Logger
{
    public  class Log4NetLoggerImpl : ILog4NetLogger
    {
        private log4net.ILog _logger;
        private Log4NetLoggerImpl(log4net.ILog logger)
        {
            _logger = logger;
        }

        public static Log4NetLoggerImpl CreateLogger()
        {
            string configPath = ConfigurationManager.AppSettings["Log4NetConfigFile"];
            if (!File.Exists(configPath))
            {
                throw new ArgumentException("Does not exist.", nameof(configPath));
            }

            log4net.Config.XmlConfigurator.Configure(new FileInfo(configPath));
            var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            return new Log4NetLoggerImpl(logger);
        }
        public void Info(string message)
        {
            _logger.Info(message);
        }
        public void Debug(string message)
        {
            _logger.Debug(message);
        }
        public void Warn(string message)
        {
            _logger.Warn(message);
        }
        public void Error(string message)
        {
            _logger.Error(message);
        }
        public void Fatal(string message)
        {
            _logger.Fatal(message);
        }
        
       
    }
}
