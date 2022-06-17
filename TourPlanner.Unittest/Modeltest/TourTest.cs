using NUnit.Framework;
using System;
using TourPlanner;
using TourPlanner.DataAccessLayer.Database;
using TourPlanner.Models;

namespace TourPlanner.Unittest.Modeltest
{
    public class TourTest
    {
        [Test]
        public void Test_AssignInvalidValueOfTransportType()
        {
            var tour = new Tour("Tour1", "Description1", "Vienna", "Graz", "asdf", 120.2, new TimeSpan(0,1,20,20));
            Assert.AreEqual("Car", tour.TransportType);
        }

        [Test]
        public void Test_AssignNoName()
        {
            var tour = new Tour(string.Empty, "description1", "Vienna", "Graz", "Car", 120.2, new TimeSpan(0, 1, 20, 20));
            Assert.AreEqual("Vienna-Graz", tour.Name);
        }

        [Test]
        public void Test_AssignAllValues()
        {
            var tour = new Tour("Tour1", "Description1", "Vienna", "Graz", "Car", 120.2, new TimeSpan(1, 20, 20));
            Assert.AreEqual("Tour1", tour.Name);
            Assert.AreEqual("Description1", tour.Description);
            Assert.AreEqual("Vienna", tour.Start);
            Assert.AreEqual("Graz", tour.Destination);
            Assert.AreEqual("Car", tour.TransportType);
            Assert.AreEqual(120.2, tour.Distance);
            Assert.AreEqual(TimeSpan.Parse("1:20:20"), tour.Time);
        }
    }
}