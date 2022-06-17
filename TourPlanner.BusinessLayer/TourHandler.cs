using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TourPlanner.BusinessLayer.JsonClasses;
using TourPlanner.DataAccessLayer.Database;
using TourPlanner.DataAccessLayer.FileSystem;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    public class TourHandler : ITourHandler {

        private IDatabase _database;
        private IFileSystem _fileSystem;

        private static ITourHandler? handler;

        private TourHandler() {
            _database = Database.GetDatabase();
            _fileSystem = FileSystem.GetFileSystem();
        }

        public static ITourHandler GetHandler()
        {
            if (handler == null)
            {
                handler = new TourHandler();
            }
            return handler;
        }

        private TourHandler(IDatabase database, IFileSystem fileSystem)
        {
            _database = database;
            _fileSystem = fileSystem;
        }

        public static ITourHandler GetHandler(IDatabase database, IFileSystem fileSystem)
        {
            if (handler == null)
            {
                handler = new TourHandler(database, fileSystem);
            }
            return handler;
        }

        public void AddNewTour(Tour newTour)
        {
            _database.AddNewTour(newTour);

            int currentIncrementValue = _database.GetCurrentIncrementValue();
            _fileSystem.SaveImageFile(newTour.Start, newTour.Destination, currentIncrementValue);
        }
           

        public void AddNewTourLog(int tourId, TourLog newTourLog)
        {
            _database.AddNewTourLog(tourId, newTourLog);
        }

        public IEnumerable<Tour> GetTours() {
            return _database.GetTours();
        }

        public IEnumerable<TourLog> GetTourLogs(Tour tour)
        {
            return _database.GetTourLogs(tour);
        }

        public List<TourLog> GetAllTourLogs()
        {
            return _database.GetAllTourLogs();
        }

        public ImageSource GetImageFile(Tour tour)
        {
            return _fileSystem.LoadImageFile(tour);
        }

        public IEnumerable<Tour> SearchForTour(string searchItem) {
            return _database.SearchTours(searchItem.ToLower());
        }

        public void ModifyTour(int id, Tour modifiedTour)
        {
            _database.ModifyTour(id, modifiedTour);
        }

        public void ModifyTourLog(TourLog tourLog)
        {
            _database.ModifyTourLog(tourLog);
        }

        public void DeleteTour(Tour deleteTour)
        {
            _database.DeleteTour(deleteTour);
            _fileSystem.DeleteImageFile(deleteTour);
        }

        public void DeleteTourLog(TourLog deleteTourLog)
        {
            _database.DeleteTourLog(deleteTourLog);
        }
    } 
}
