using System;

namespace TourPlanner.Models
{
    public class Tour
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string TransportType { get; set; }
        public double Distance { get; set; }
        public TimeSpan Time { get; set; }

        public Tour(string name, string description, string from, string to, string transportType, double distance, TimeSpan time)
        {
            Name = name;
            Description = description;
            From = from;
            To = to;
            TransportType = transportType;
            Distance = distance;
            Time = time;
        }

        public Tour(int id, string name, string description, string from, string to, string transportType, double distance, TimeSpan time)
        {
            Id = id;
            Name = name;
            Description = description;
            From = from;
            To = to;
            TransportType = transportType;
            Distance = distance;
            Time = time;
        }
    }
}
