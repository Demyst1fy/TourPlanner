using System;
using TourPlanner.Models.Enums;

namespace TourPlanner.Models
{
    public class TourLog
    {
        public int TourId { get; set; }
        public int Id { get; set; }
        public DateTime Datetime { get; set; }
        public string Comment { get; set; }
        public Difficulty Difficulty { get; set; }
        public TimeSpan TotalTime { get; set; }
        public int Rating { get; set; }

        public TourLog(int id, string comment, Difficulty difficulty, TimeSpan totalTime, int rating)
        {
            Id = id;
            Datetime = DateTime.Now;
            Comment = comment;
            Difficulty = difficulty;
            TotalTime = totalTime;
            Rating = rating;
        }

        public TourLog(int id, DateTime datetime, string comment, Difficulty difficulty, TimeSpan totalTime, int rating)
        {
            Id = id;
            Datetime = datetime;
            Comment = comment;
            Difficulty = difficulty;
            TotalTime = totalTime;
            Rating = rating;
        }

        public TourLog(string comment, Difficulty difficulty, TimeSpan totalTime, int rating)
        {
            Datetime = DateTime.Now;
            Comment = comment;
            Difficulty = difficulty;
            TotalTime = totalTime;
            Rating = rating;
        }
    }
}
