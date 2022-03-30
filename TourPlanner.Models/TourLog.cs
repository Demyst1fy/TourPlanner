using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Models
{
    public class TourLog
    {
        public int Id { get; set; }
        public DateTime Datetime { get; set; }
        public string Comment { get; set; }
        public int Difficulty { get; set; }
        public string TotalTime { get; set; }
        public int Rating { get; set; }

        public TourLog(int id, string comment, int difficulty, string totalTime, int rating)
        {
            Id = id;
            Datetime = DateTime.Now;
            Comment = comment;
            Difficulty = difficulty;
            TotalTime = totalTime;
            Rating = rating;
        }
    }
}
