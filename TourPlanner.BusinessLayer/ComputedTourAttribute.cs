using System;
using System.Collections.Generic;
using System.Linq;
using TourPlanner.DictionaryHandler;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    public static class ComputedTourAttribute
    {
        public static double CalculatePopularity(ITourHandler tourHandler, Tour currentTour)
        {
            var currentTourLogs = tourHandler.GetTourLogs(currentTour);
            double computedValue = (double)currentTourLogs.Count() / tourHandler.GetAllTourLogs().Count;
            return Math.Round(computedValue * 100.0, 0);
        }

        public static double CalculateChildFriendliness(ITourHandler tourHandler, ITourDictionary tourDictionary, Tour currentTour)
        {
            var currentTourLogs = tourHandler.GetTourLogs(currentTour);
            int difficultySumFromCurrentTourLogs = GetSumOfDifficulty(currentTourLogs, tourDictionary);
            int durationIndexFromTour = GetDurationIndex(currentTour);
            int distanceIndexFromTour = GetDistanceIndex(currentTour);

            double computedValue = (double)(currentTourLogs.Count() + durationIndexFromTour + distanceIndexFromTour) / (difficultySumFromCurrentTourLogs + 5 + 5);

            return Math.Round(computedValue * 100, 0);
        }

        private static int GetSumOfDifficulty(IEnumerable<TourLog> tourLogList, ITourDictionary tourDictionary)
        {
            int difficultySum = 0;
            foreach (var tourLog in tourLogList)
            {
                if (tourLog.Difficulty == tourDictionary.GetResourceFromDictionary("StringTourLogsDifficultyEasy"))
                    difficultySum += 1;
                else if (tourLog.Difficulty == tourDictionary.GetResourceFromDictionary("StringTourLogsDifficultyMedium"))
                    difficultySum += 2;
                else if (tourLog.Difficulty == tourDictionary.GetResourceFromDictionary("StringTourLogsDifficultyHard"))
                    difficultySum += 3;
                else
                    difficultySum += 1;
            }
            return difficultySum;
        }

        private static int GetDurationIndex(Tour currentTour)
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

        private static int GetDistanceIndex(Tour currentTour)
        {
            if (currentTour.Distance < 60)
                return 5;
            else if (currentTour.Distance >= 60 && currentTour.Distance < 120)
                return 4;
            else if (currentTour.Distance >= 120 && currentTour.Distance < 180)
                return 3;
            else if (currentTour.Distance >= 180 && currentTour.Distance < 240)
                return 2;
            else
                return 1;
        }
    }
}
