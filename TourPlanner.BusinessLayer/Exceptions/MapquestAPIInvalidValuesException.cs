using System;
using TourPlanner.BusinessLayer.TourAttributes;

namespace TourPlanner.BusinessLayer.Exceptions
{
    public class MapquestAPIInvalidValuesException : Exception
    {
        public double InvalidDistance { get; set; }
        public int InvalidTime { get; set; }

        public MapquestAPIInvalidValuesException(string message) : base(message)
        {
        }

        public MapquestAPIInvalidValuesException(string message, double invalidDistance, int invalidTime) : this(message)
        {
            InvalidDistance = invalidDistance;
            InvalidTime = invalidTime;
        }
    }
}
