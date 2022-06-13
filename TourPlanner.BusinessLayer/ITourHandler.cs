using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer {
    public interface ITourHandler {
        IEnumerable<Tour> GetTours();
        IEnumerable<TourLog> GetTourLogs(Tour tour);
        IEnumerable<Tour> SearchForTour(string itemName, bool caseSensitive = false);
        Task<Tour?> GetTourFromAPI(string name, string description, string start, string end, string transportType);
        void AddNewTour(Tour newTour);
        void AddNewTourLog(int tourId, TourLog newTourLog);
        void ModifyTour(int id, Tour newTour);
        void ModifyTourLog(TourLog tourLog);
        void DeleteTour(Tour deleteTour);
        void DeleteTourLog(TourLog deleteTourLog);
        ImageSource GetImageFile(Tour tour);
    }
}
