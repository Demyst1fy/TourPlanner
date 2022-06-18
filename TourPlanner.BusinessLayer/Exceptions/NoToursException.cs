using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.BusinessLayer.Exceptions
{
    public class NoToursException : Exception
    {
        public NoToursException(string message) : base(message)
        {
        }
    }
}
