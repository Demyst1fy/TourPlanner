using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public interface IDataAccess
    {
        List<Tour> GetTours();
        void AddNewTour(Tour newTour);
        void ModifyTour(int id, Tour newTour);
        void DeleteTour(Tour newTour);
    }
}
