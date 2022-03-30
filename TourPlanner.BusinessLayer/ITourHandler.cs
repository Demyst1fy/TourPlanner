using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer {
    public interface ITourHandler {
        IEnumerable<Tour> GetTours();
        IEnumerable<Tour> SearchForTour(string itemName, bool caseSensitive = false);
        void AddNewTour(Tour newTour);
    }
}
