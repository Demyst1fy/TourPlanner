using System;

namespace TourPlanner.Models
{
    public class Tour
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Description { get; set; }
        public string TransportType { get; set; }
        public int Distance { get; set; }
        public string Time { get; set; }

        public Tour(string start, string end, string description, int id = 5, int distance = 100, string transportType = "Car", string time = "4:00:00")
        {
            Id = id;
            Name = start + "-" + end;
            Start = start;
            End = end;
            Description = description;
            Distance = distance;
            TransportType = transportType;
            Time = time;
        }
    }
}
