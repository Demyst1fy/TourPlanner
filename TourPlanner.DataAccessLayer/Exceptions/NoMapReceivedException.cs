using System;

namespace TourPlanner.DataAccessLayer.Exceptions
{
    public class NoMapReceivedException : Exception
    {
        public NoMapReceivedException(string message) : base(message)
        {
        }
    }
}
