using NUnit.Framework;
using System;
using TourPlanner;
using TourPlanner.DataAccessLayer.Database;
using TourPlanner.Models;

namespace TourPlanner.Unittest.Modeltest
{
    public class TourLogTest
    {
        [Test]
        public void Test_AssignAllValues()
        {
            var tourLog = new TourLog("Comment1", "Medium", new TimeSpan(1,40,20), 4);

            Assert.AreEqual(0, tourLog.Id);
            Assert.AreEqual(DateTime.Now.Hour, tourLog.Datetime.Hour);

            Assert.AreEqual("Comment1", tourLog.Comment);
            Assert.AreEqual("Medium", tourLog.Difficulty);
            Assert.AreEqual(TimeSpan.Parse("1:40:20"), tourLog.TotalTime);
            Assert.AreEqual(4, tourLog.Rating);
        }
    }
}