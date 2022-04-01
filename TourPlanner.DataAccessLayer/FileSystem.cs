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

        private static IDataAccess? _database;

        private FileSystem()
        {
            // get connection data from config file
            filePath = "...";
            // establish connection with db
            tours = new List<Tour>() {
                new Tour("Vienna", "Graz", "Cool Tour"),
                new Tour("Vienna", "Salzburg", "Awesome Tour"),
                new Tour("Salzburg", "Vienna", "A Tour"),
                new Tour("Graz", "Vienna", "Another Tour"),
            };
        }
        public static IDataAccess GetFileSystem()
        {
            // get connection data from config file
            if (_database == null)
            {
                _database = new FileSystem();
            }
            return _database;
        }

        public IEnumerable<Tour> GetTours()
        {
            // get from file


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
