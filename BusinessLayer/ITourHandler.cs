using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer {
    public interface ITourHandler {
        IEnumerable<Tour> GetItems();
        IEnumerable<Tour> SearchForTour(string itemName, bool caseSensitive = false);
    }
}
