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

        public void AddNewTour(Tour newTour)
        {
            database.AddNewTour(newTour);

            int currentIncrementValue = database.GetCurrentIncrementValue();
            fileSystem.SaveImageFile(newTour.Start, newTour.Destination, currentIncrementValue);
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

        public List<TourLog> GetAllTourLogs()
        {
            return database.GetAllTourLogs();
        }

        public ImageSource GetImageFile(Tour tour)
        {
            return fileSystem.LoadImageFile(tour);
        }

        public IEnumerable<Tour> SearchForTour(string searchItem) {
            return database.SearchTours(searchItem.ToLower());
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

        public double CalculatePopularity(Tour currentTour)
        {
            var currentTourLogs = GetTourLogs(currentTour);
            double computedValue = (double)currentTourLogs.Count() / GetAllTourLogs().Count;
            return Math.Round(computedValue * 100.0, 0);
        }

        public double CalculateChildFriendliness(Tour currentTour)
        {
            var currentTourLogs = GetTourLogs(currentTour);
            int difficultySumFromCurrentTourLogs = GetSumOfDifficulty(currentTourLogs);
            int durationIndexFromTour = GetDurationIndex(currentTour);
            int distanceIndexFromTour = GetDistanceIndex(currentTour);

            double computedValue = (double)(currentTourLogs.Count() + durationIndexFromTour + distanceIndexFromTour) / (difficultySumFromCurrentTourLogs + 5 + 5);

            return Math.Round(computedValue * 100, 0);
        }

        private int GetSumOfDifficulty(IEnumerable<TourLog> tourLogList)
        {
            int difficultySum = 0;
            foreach (var tourLog in tourLogList)
            {
                if (tourLog.Difficulty == (string)Application.Current.Resources["StringTourLogsDifficultyEasy"])
                    difficultySum += 3;
                else if (tourLog.Difficulty == (string)Application.Current.Resources["StringTourLogsDifficultyMedium"])
                    difficultySum += 2;
                else if (tourLog.Difficulty == (string)Application.Current.Resources["StringTourLogsDifficultyHard"])
                    difficultySum += 1;
                else
                    difficultySum += 3;
            }
            return difficultySum;
        }

        private int GetDurationIndex(Tour currentTour)
        {
            if (currentTour.Time.TotalSeconds < 1800)
                return 5;
            else if (currentTour.Time.TotalSeconds >= 1800 && currentTour.Time.TotalSeconds < 3600)
                return 4;
            else if (currentTour.Time.TotalSeconds >= 3600 && currentTour.Time.TotalSeconds < 5400)
                return 3;
            else if (currentTour.Time.TotalSeconds >= 5400 && currentTour.Time.TotalSeconds < 7200)
                return 2;
            else
                return 1;
        }

        private int GetDistanceIndex(Tour currentTour)
        {
            if (currentTour.Distance < 60)
                return 5;
            else if (currentTour.Distance >= 60 && currentTour.Distance < 120)
                return 4;
            else if (currentTour.Distance >= 120 && currentTour.Distance < 180)
                return 3;
            else if (currentTour.Distance >= 240 && currentTour.Distance < 300)
                return 2;
            else
                return 1;
        }
    } 
}
