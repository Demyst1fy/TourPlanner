using System;

namespace TourPlanner.Models
{
    public class Tour
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string TransportType { get; set; }
        public string Time { get; set; }

        public Tour(string start, string end)
        {
            Name = start + "-" + end;
            Start = start;
            End = end;
        }
    }
}
