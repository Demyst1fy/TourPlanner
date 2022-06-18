using TourPlanner.BusinessLayer.Logger;
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
                tourHandler = TourHandlerImpl.CreateHandler();
            }
            return tourHandler;
        }

        public static ITourHandler GetHandler(IDatabase database, IFileSystem fileSystem, ILog4NetLogger logger)
        {
            if (tourHandler == null)
            {
                tourHandler = TourHandlerImpl.CreateHandler(database, fileSystem, logger);
            }
            return tourHandler;
        }
    }
}
