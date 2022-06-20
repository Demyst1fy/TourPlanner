using System;
using System.Collections.Generic;

namespace TourPlanner.Models
{
    public class Tour
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Start { get; set; }
        public string Destination { get; set; }
        public string TransportType { get; set; }
        public double Distance { get; set; }
        public TimeSpan Time { get; set; }

        public Tour() { }

        public Tour(string name, string description, string start, string destination, string transportType, double distance, TimeSpan time)
        {
            if (string.IsNullOrEmpty(name))
                Name = $"{start}-{destination}";
            else
                Name = name;

            Description = description;
            Start = start;
            Destination = destination;

            TransportType = transportType switch
            {
                "Car" => "Car",
                "Foot" => "Foot",
                "Bicycle" => "Bicycle",
                _ => "Car",
            };

            Distance = distance;
            Time = time;
        }

        public Tour(int id, string name, string description, string start, string destination, string transportType, double distance, TimeSpan time)
        {
            Id = id;
            Name = name;
            Description = description;
            Start = start;
            Destination = destination;
            TransportType = transportType;
            Distance = distance;
            Time = time;
        }
    }
}
