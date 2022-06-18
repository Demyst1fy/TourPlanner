using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.BusinessLayer.Exceptions
{
    public class TourAlreadyExistsException : Exception
    {
        public TourAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
