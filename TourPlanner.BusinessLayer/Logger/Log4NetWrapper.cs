﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace TourPlanner.Logger
{
    public  class Log4NetWrapper : ILoggerWrapper
    {
        private log4net.ILog _logger;
        Log4NetWrapper(log4net.ILog logger)
        {
            _logger = logger;
        }

        public static Log4NetWrapper CreateLogger(string configPath)
        {
            if (!File.Exists(configPath))
            {
                throw new ArgumentException("Does not exist.", nameof(configPath));
            }

            log4net.Config.XmlConfigurator.Configure(new FileInfo(configPath));
            var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            return new Log4NetWrapper(logger);
        }
        public void Info(string message)
        {
            _logger.Info(message);
        }
        public void Debug(string message)
        {
            this.Debug(message);
        }
        public void Warn(string message)
        {
            _logger.Warn(message);
        }
        public void Error(string message)
        {
            this.Error(message);
        }
        public void Fatal(string message)
        {
            _logger.Fatal(message);
        }
        
       
    }
}
