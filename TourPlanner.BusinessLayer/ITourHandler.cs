using System.Collections.Generic;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer {
    public interface ITourHandler {
        IEnumerable<Tour> GetTours();
        IEnumerable<Tour> SearchForTour(string itemName, bool caseSensitive = false);
        Task<Tour?> GetTourFromAPI(string name, string description, string start, string end, string transportType);
        void AddNewTour(Tour newTour);
        void ModifyTour(int id, Tour newTour);
        void DeleteTour(Tour deleteTour);
        string GetImage(string start, string end);
    }
}
