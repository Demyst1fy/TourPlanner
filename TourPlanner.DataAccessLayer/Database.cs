using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    class Database : IDataAccess
    {

        private string connectionString;
        private List<Tour> tours;
        public Database()
        {
            // get connection data from config file
            connectionString = "...";
            // establish connection with db
            tours = new List<Tour>() {
                new Tour("Vienna", "Graz", "Cool Tour"),
                new Tour("Vienna", "Salzburg", "Awesome Tour"),
                new Tour("Salzburg", "Vienna", "A Tour"),
                new Tour("Graz", "Vienna", "Another Tour"),
            };
        }

        public IEnumerable<Tour> GetTours()
        {
            // select SQL query
            

            return tours;
        }

        public void AddNewTour(Tour newTour)
        {
            tours.Add(newTour);
        }
    }
}
