using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DataAccessLayer.Database;
using TourPlanner.DataAccessLayer.FileSystem;

namespace TourPlanner.DataAccessLayer.Database
{
    public static class DatabaseRepository
    {
        private static IDatabase database;

        public static IDatabase GetDatabase()
        {
            if (database == null)
            {
                database = Database.CreateDatabase();
            }
            return database;
        }
    }
}
