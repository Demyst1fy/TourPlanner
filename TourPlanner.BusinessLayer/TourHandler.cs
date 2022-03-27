using System.Collections.Generic;
using System.Linq;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    internal class TourHandler : ITourHandler {

        public IEnumerable<Tour> GetItems() {
            // usually querying the disk, or from a DB, or ...
            return new List<Tour>() {
                new Tour("Vienna", "Graz"),
                new Tour("Vienna", "Salzburg"),
                new Tour("Salzburg", "Vienna"),
                new Tour("Graz", "Vienna"),
            };
        }

        public IEnumerable<Tour> SearchForTour(string itemName, bool caseSensitive = false) {
            IEnumerable<Tour> items = GetItems();

            if (caseSensitive) {
                return items.Where(x => x.Name.Contains(itemName));
            }
            return items.Where(x => x.Name.ToLower().Contains(itemName.ToLower()));
        }
    }
}
