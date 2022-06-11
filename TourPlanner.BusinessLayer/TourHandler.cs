using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using TourPlanner.BusinessLayer.JsonClasses;
using TourPlanner.DataAccessLayer;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    public class TourHandler : ITourHandler {

        private IDataAccess database;

        private static ITourHandler? handler;

        private TourHandler() {
            database = Database.GetDatabase();
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
            TourDistanceAndTime value = await APIRequest.GetRequest(start, end, transportType);

            /*if (value == null)
                return null;*/

            double distance = value.Distance;
            TimeSpan time = TimeSpan.FromSeconds(value.Time);

            if(string.IsNullOrEmpty(name))
                name = $"{start}-{end}";

            return new Tour(name, description, start, end, transportType, distance, time);
        }

        public void AddNewTour(Tour newTour)
        {
            database.AddNewTour(newTour);
        }

        public IEnumerable<Tour> GetTours() {
            return database.GetTours();
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

        public void DeleteTour(Tour deleteTour)
        {
            database.DeleteTour(deleteTour);
        }

        public string GetImage(string start, string end)
        {
            var apikey = ConfigurationManager.AppSettings["mapquestapikey"];

            return $"https://www.mapquestapi.com/staticmap/v5/map?start={start}&end={end}&key={apikey}";
        }
    } 
}
