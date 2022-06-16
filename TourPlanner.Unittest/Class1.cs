using NUnit.Framework;
using System;
using System.Configuration;
using TourPlanner;
using TourPlanner.DataAccessLayer.Database;
using TourPlanner.Models;

namespace TourPlanner.Unittest.Modeltest
{
    public class DatabaseTest
    {
        [Test]
        public void TestTour_AssignInvalidValueOfTransportType()
        {
            var k = ConfigurationManager.AppSettings["MapImagesPath"];
            Assert.AreEqual("./MapImages", k);
        }

    }
}