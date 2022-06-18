using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DataAccessLayer.Database;
using TourPlanner.DataAccessLayer.FileSystem;

namespace TourPlanner.BusinessLayer.Logger
{
    public static class Log4NetLoggerFactory
    {
        private static ILog4NetLogger logger;

        public static ILog4NetLogger GetLogger()
        {
            if (logger == null)
            {
                logger = Log4NetLoggerImpl.CreateLogger();
            }
            return logger;
        }
    }
}
