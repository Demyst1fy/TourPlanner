using System;

namespace TourPlanner.DataAccessLayer.Exceptions
{
    public class NoMapImageFileFoundException : Exception
    {
        public NoMapImageFileFoundException(string message) : base(message)
        {
        }
    }
}
