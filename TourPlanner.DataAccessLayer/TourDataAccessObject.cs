using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public class TourDataAccessObject
    {
        private IDataAccess dataAccess;

        public TourDataAccessObject()
        {
            // check which datasource to use
            // dataAccess = new FileSystem();
            dataAccess = new Database();
        }

        public void AddNewTour(Tour newTour)
        {
            dataAccess.AddNewTour(newTour);
        }

        public IEnumerable<Tour> GetTours()
        {
            return dataAccess.GetTours();
        }

        public void DeleteTour(Tour deleteTour)
        {
            dataAccess.DeleteTour(deleteTour);
        }
    }
}
