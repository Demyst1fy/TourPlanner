using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DataAccessLayer.Database;
using TourPlanner.DataAccessLayer.FileSystem;

namespace TourPlanner.BusinessLayer.TourHandler
{
    public static class TourHandlerFactory
    {
        private static ITourHandler tourHandler;

        public static ITourHandler GetHandler()
        {
            if (tourHandler == null)
            {
                tourHandler = new TourHandlerImpl();
            }
            return tourHandler;
        }

        public static ITourHandler GetHandler(IDatabase database, IFileSystem fileSystem)
        {
            if (tourHandler == null)
            {
                tourHandler = new TourHandlerImpl(database, fileSystem);
            }
            return tourHandler;
        }
    }
}
