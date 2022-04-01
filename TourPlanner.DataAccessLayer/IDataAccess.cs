using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public interface IDataAccess
    {
        public IEnumerable<Tour> GetTours();
        public void AddNewTour(Tour newTour);
        void DeleteTour(Tour newTour);
    }
}
