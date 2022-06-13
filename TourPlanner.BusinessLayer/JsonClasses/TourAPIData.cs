namespace TourPlanner.BusinessLayer.JsonClasses
{
    public class TourAPIData
    {
        public double Distance { get; set; }
        public int Time { get; set; }

        public TourAPIData(double distance, int time)
        {
            Distance = distance;
            Time = time;
        }
    }
}
