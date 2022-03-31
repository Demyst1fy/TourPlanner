using System.Collections.Generic;
using System.Linq;
using TourPlanner.DataAccessLayer;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    internal class TourHandler : ITourHandler {

        private TourDataAccessObject tourDataAccessObject = new TourDataAccessObject();

        public void AddNewTour(Tour newTour)
        {
            tourDataAccessObject.AddNewTour(newTour);
        }

        public IEnumerable<Tour> GetTours() {
            return tourDataAccessObject.GetTours();
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
            tourDataAccessObject.DeleteTour(deleteTour);
        }
    }
}
