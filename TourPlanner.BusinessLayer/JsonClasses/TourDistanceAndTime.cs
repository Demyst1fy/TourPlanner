using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.BusinessLayer.JsonClasses
{
    public class TourDistanceAndTime
    {
        public double Distance { get; set; }
        public int Time { get; set; }

        public TourDistanceAndTime(double distance, int time)
        {
            Distance = distance;
            Time = time;
        }
    }
}
