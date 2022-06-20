using System;
using TourPlanner.Models;
using System.IO;
using TourPlanner.BusinessLayer.TourHandler;
using Newtonsoft.Json;
using System.Configuration;
using TourPlanner.BusinessLayer.Exceptions;
using TourPlanner.BusinessLayer.Logger;

namespace TourPlanner.BusinessLayer.ExportImport
{
    public static class JsonFileHandler
    {
        public static bool ExportTour(Tour tour)
        {
            if(tour == null) 
                return false;

            string exportedJsonPath = ConfigurationManager.AppSettings["ExportedJsonPath"];

            Directory.CreateDirectory(exportedJsonPath);

            // Serialize Tour tour and save it into a .txt into current directory
            string jsonString = JsonConvert.SerializeObject(new
            {
                Name = tour.Name,
                Description = tour.Description,
                Start = tour.Start,
                Destination = tour.Destination,
                TransportType = tour.TransportType,
                Distance = tour.Distance,
                Time = tour.Time.TotalSeconds,
            });

            File.WriteAllText($"{exportedJsonPath}/{tour.Name}.json", jsonString);
            return true;
        }

        public static int ImportTour(ITourHandler tourHandler, ILog4NetLogger logger, string jsonFile)
        {
            try
            {
                string jsonString = File.ReadAllText(jsonFile);

                if (string.IsNullOrEmpty(jsonString))
                {
                    logger.Error($"File: {jsonFile} is empty");
                    return -1;
                }

                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonString);

                string Name = jsonObject.Name;
                string Description = jsonObject.Description;
                string Start = jsonObject.Start;
                string Destination = jsonObject.Destination;
                string TransportType = jsonObject.TransportType;
                double Distance = jsonObject.Distance;
                TimeSpan Time = TimeSpan.FromSeconds((double)jsonObject.Time);

                Tour importedTour = new Tour(Name, Description, Start, Destination, TransportType, Distance, Time);

                tourHandler.AddNewTour(importedTour);
                logger.Info($"Tour imported successfully - {Name}");
                return 0;
            }
            catch (TourAlreadyExistsException ex)
            {
                logger.Error($"Imported Tour already exist in the database: {ex.Message}");
                return -2;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return -3;
            }
        }
    }
}

