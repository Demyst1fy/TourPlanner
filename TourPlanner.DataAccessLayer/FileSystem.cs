using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    class FileSystem : IDataAccess
    {
        private string filePath;
        private List<Tour> tours;

        public FileSystem()
        {
            // get filepath data from config file
            filePath = "...";
        }

        public IEnumerable<Tour> GetTours()
        {
            // select SQL query
            tours = new List<Tour>() {
                new Tour("Vienna", "Graz", "Cool Tour"),
                new Tour("Vienna", "Salzburg", "Awesome Tour"),
                new Tour("Salzburg", "Vienna", "A Tour"),
                new Tour("Graz", "Vienna", "Another Tour"),
            };

            return tours;
        }

        public void AddNewTour(Tour newTour)
        {
            tours.Add(newTour);
        }

        public void DeleteTour(Tour newTour)
        {
            tours.RemoveAll(x => x.Name == newTour.Name);
        }
    }
}
