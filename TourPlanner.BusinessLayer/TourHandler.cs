using System.Collections.Generic;
using System.Linq;
using TourPlanner.DataAccessLayer;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    public class TourHandler : ITourHandler {

        private IDataAccess database;

        private static ITourHandler? handler;

        private TourHandler() {
            database = Database.GetDatabase();
        }

        public static ITourHandler GetHandler()
        {
            if (handler == null)
            {
                handler = new TourHandler();
            }
            return handler;
        }

        public void AddNewTour(Tour newTour)
        {
            database.AddNewTour(newTour);
        }

        public IEnumerable<Tour> GetTours() {
            return database.GetTours();
        }

        public IEnumerable<Tour> SearchForTour(string itemName, bool caseSensitive = false) {
            IEnumerable<Tour> items = GetTours();

            if (caseSensitive) {
                return items.Where(x => x.Name.Contains(itemName));
            }
            return items.Where(x => x.Name.ToLower().Contains(itemName.ToLower()));
        }

        public void DeleteTour(Tour deleteTour)
        {
            database.DeleteTour(deleteTour);
        }
    }
}
