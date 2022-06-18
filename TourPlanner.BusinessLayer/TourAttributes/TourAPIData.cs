using System.Collections.Generic;

namespace TourPlanner.BusinessLayer.TourAttributes
{
    public class TourAPIData
    {
        public int StatusCode { get; set; }
        public List<object> Message { get; set; }
        public double Distance { get; set; }
        public int Time { get; set; }

        public TourAPIData(int statusCode, List<object> message, double distance, int time)
        {
            StatusCode = statusCode;
            Message = message;
            Distance = distance;
            Time = time;
        }
    }
}
