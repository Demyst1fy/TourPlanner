using System;

namespace TourPlanner.DataAccessLayer.Exceptions
{
    public class NoMapImageFileFound : Exception
    {
        public NoMapImageFileFound(string message) : base(message)
        {
        }
    }
}
