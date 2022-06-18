using System;
using TourPlanner.BusinessLayer.TourAttributes;

namespace TourPlanner.BusinessLayer.Exceptions
{
    public class MapquestAPIErrorException : Exception
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public MapquestAPIErrorException(string message) : base(message)
        {
        }

        public MapquestAPIErrorException(string message, int errorCode, string errorMessage) : this(message)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}
