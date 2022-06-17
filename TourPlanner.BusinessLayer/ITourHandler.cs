using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer {
    public interface ITourHandler {
        IEnumerable<Tour> GetTours();
        IEnumerable<TourLog> GetTourLogs(Tour tour);
        List<TourLog> GetAllTourLogs();
        void AddNewTour(Tour newTour);
        void AddNewTourLog(int tourId, TourLog newTourLog);
        void ModifyTour(int id, Tour newTour);
        void ModifyTourLog(TourLog tourLog);
        void DeleteTour(Tour deleteTour);
        void DeleteTourLog(TourLog deleteTourLog);
        IEnumerable<Tour> SearchForTour(string itemName);
        ImageSource GetImageFile(Tour tour);
    }
}
