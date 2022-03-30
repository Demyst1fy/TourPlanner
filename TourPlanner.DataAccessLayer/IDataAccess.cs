using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    interface IDataAccess
    {
        public IEnumerable<Tour> GetTours();
        public void AddNewTour(Tour newTour);
    }
}
