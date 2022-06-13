using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using TourPlanner.BusinessLayer.JsonClasses;
using TourPlanner.DataAccessLayer.Database;
using TourPlanner.DataAccessLayer.FileSystem;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    public class TourHandler : ITourHandler {

        private IDatabase database;
        private IFileSystem fileSystem;

        private static ITourHandler? handler;

        private TourHandler() {
            database = Database.GetDatabase();
            fileSystem = FileSystem.GetFileSystem();
        }

        public static ITourHandler GetHandler()
        {
            if (handler == null)
            {
                handler = new TourHandler();
            }
            return handler;
        }

        public async Task<Tour?> GetTourFromAPI(string name, string description, string start, string end, string transportType)
        {
            TourAPIData value = await APIRequest.RequestDirection(start, end, transportType);

            /*if (value == null)
                return null;*/

            double distance = value.Distance;
            TimeSpan time = TimeSpan.FromSeconds(value.Time);

            if (string.IsNullOrEmpty(name))
                name = $"{start}-{end}";

            return new Tour(name, description, start, end, transportType, distance, time);
        }

        public void AddNewTour(Tour newTour)
        {
            database.AddNewTour(newTour);

            int currentIncrementValue = database.GetCurrentIncrementValue();
            fileSystem.SaveImageFile(newTour.From, newTour.To, currentIncrementValue);
        }

        public void AddNewTourLog(int tourId, TourLog newTourLog)
        {
            database.AddNewTourLog(tourId, newTourLog);
        }

        public IEnumerable<Tour> GetTours() {
            return database.GetTours();
        }

        public IEnumerable<TourLog> GetTourLogs(Tour tour)
        {
            return database.GetTourLogs(tour);
        }

        public ImageSource GetImageFile(Tour tour)
        {
            return fileSystem.LoadImageFile(tour);
        }

        public IEnumerable<Tour> SearchForTour(string itemName, bool caseSensitive = false) {
            IEnumerable<Tour> items = GetTours();

            if (caseSensitive) {
                return items.Where(x => x.Name.Contains(itemName));
            }
            return items.Where(x => x.Name.ToLower().Contains(itemName.ToLower()));
        }

        public void ModifyTour(int id, Tour modifiedTour)
        {
            database.ModifyTour(id, modifiedTour);
        }

        public void ModifyTourLog(TourLog tourLog)
        {
            database.ModifyTourLog(tourLog);
        }

        public void DeleteTour(Tour deleteTour)
        {
            database.DeleteTour(deleteTour);
            fileSystem.DeleteImageFile(deleteTour);
        }

        public void DeleteTourLog(TourLog deleteTourLog)
        {
            database.DeleteTourLog(deleteTourLog);
        }
    } 
}
