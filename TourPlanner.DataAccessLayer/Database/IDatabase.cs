using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer.Database
{
    public interface IDatabase
    {
        List<Tour> GetTours();
        List<TourLog> GetTourLogs(Tour tour);
        void AddNewTour(Tour newTour);
        void AddNewTourLog(int tourId, TourLog newTourLog);
        void ModifyTour(int id, Tour newTour);
        void ModifyTourLog(TourLog tourLog);
        void DeleteTour(Tour newTour);
        void DeleteTourLog(TourLog deleteTourLog);
        int GetCurrentIncrementValue();
    }
}
