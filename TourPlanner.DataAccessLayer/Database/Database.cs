using System;
using System.Collections.Generic;
using System.Configuration;
using TourPlanner.Models;
using Npgsql;

namespace TourPlanner.DataAccessLayer.Database
{
    public class Database : IDatabase
    {

        public string _conString { get; set; }
        private string _dbname = "tourplanner";
        private static IDatabase? _database;

        private Database()
        {
            _conString = ConfigurationManager.AppSettings["ConnectionString"] ?? "not found";

            CreateDatabaseIfNotExists();

            _conString += $";Database={_dbname}";

            CreateTablesIfNotExist();
        }

        private void CreateDatabaseIfNotExists()
        {
            string sql = $"SELECT 1 FROM pg_database WHERE datname='{_dbname}'";

            var con = ConOpen();
            var cmd = new NpgsqlCommand(sql, con);

            if (cmd.ExecuteScalar() == null)
            {
                con.Close();
                con = ConOpen();

                string sql2 = $"CREATE DATABASE {_dbname}";

                cmd = new NpgsqlCommand(sql2, con);
                cmd.ExecuteNonQuery();
            }
            con.Close();
        }

        private void CreateTablesIfNotExist()
        {
            const string sql = "CREATE TABLE IF NOT EXISTS tours(" +
                "t_id SERIAL PRIMARY KEY," +
                "t_name VARCHAR(50) UNIQUE NOT NULL," +
                "t_description VARCHAR(255)," +
                "t_from VARCHAR(50) NOT NULL," +
                "t_to VARCHAR(50) NOT NULL," +
                "t_transporttype VARCHAR(50) NOT NULL," +
                "t_distance FLOAT8 NOT NULL," +
                "t_time INT NOT NULL)";

            var con = ConOpen();
            var cmd = new NpgsqlCommand(sql, con);
            cmd.ExecuteNonQuery();
            con.Close();

            const string sql2 = "CREATE TABLE IF NOT EXISTS tourlogs(" +
                "t_id INT NOT NULL CONSTRAINT t_id REFERENCES tours ON UPDATE CASCADE ON DELETE CASCADE," +
                "tl_id SERIAL PRIMARY KEY," +
                "tl_datetime TIMESTAMP NOT NULL," +
                "tl_comment VARCHAR(255)," +
                "tl_difficulty VARCHAR(50) NOT NULL," +
                "tl_totaltime INT NOT NULL," +
                "tl_rating INT NOT NULL);";

            con = ConOpen();

            using var cmd2 = new NpgsqlCommand(sql2, con);
            cmd2.ExecuteNonQuery();
            con.Close();
        }

        private NpgsqlConnection ConOpen()
        {
            var con = new NpgsqlConnection(_conString);
            con.Open();
            return con;
        }

        public static IDatabase GetDatabase()
        {
            if (_database == null)
            {
                _database = new Database();
            }
            return _database;
        }

        public List<Tour> GetTours()
        {
            const string sql = "SELECT * FROM tours";

            var con = ConOpen();
            var cmd = new NpgsqlCommand(sql, con);
            var rdr = cmd.ExecuteReader();

            List<Tour> tours = new List<Tour>();

            while (rdr.Read())
            {
                var id = rdr.GetInt32(0);
                var name = rdr.GetString(1);
                string description;
                if (!rdr.IsDBNull(2))
                    description = rdr.GetString(2);
                else
                    description = string.Empty;
                var from = rdr.GetString(3);
                var to = rdr.GetString(4);
                var transportType = rdr.GetString(5);
                var distance = rdr.GetDouble(6);
                var time = TimeSpan.FromSeconds(rdr.GetInt32(7));

                tours.Add(new Tour(id, name, description, from, to, transportType, distance, time));
            }
            rdr.Close();
            con.Close();

            return tours;
        }

        public List<TourLog> GetTourLogs(Tour tour)
        {
            const string sql = "SELECT * FROM tourlogs WHERE t_id = @t_id";

            var con = ConOpen();
            var cmd = new NpgsqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@t_id", tour.Id);
            var rdr = cmd.ExecuteReader();

            List<TourLog> tourLogs = new List<TourLog>();

            while (rdr.Read())
            {
                var id = rdr.GetInt32(1);
                var datetime = rdr.GetDateTime(2);
                string comment;
                if (!rdr.IsDBNull(3))
                    comment = rdr.GetString(3);
                else
                    comment = string.Empty;
                var difficulty = rdr.GetString(4);
                var totalTime = TimeSpan.FromSeconds(rdr.GetInt32(5));
                var rating = rdr.GetInt32(6);

                tourLogs.Add(new TourLog(id, datetime, comment, difficulty, totalTime, rating));
            }
            rdr.Close();
            con.Close();

            return tourLogs;
        }

        public List<TourLog> GetAllTourLogs()
        {
            const string sql = "SELECT * FROM tourlogs";

            var con = ConOpen();
            var cmd = new NpgsqlCommand(sql, con);
            var rdr = cmd.ExecuteReader();

            List<TourLog> tourLogs = new List<TourLog>();

            while (rdr.Read())
            {
                var id = rdr.GetInt32(1);
                var datetime = rdr.GetDateTime(2);
                string comment;
                if (!rdr.IsDBNull(3))
                    comment = rdr.GetString(3);
                else
                    comment = string.Empty;
                var difficulty = rdr.GetString(4);
                var totalTime = TimeSpan.FromSeconds(rdr.GetInt32(5));
                var rating = rdr.GetInt32(6);

                tourLogs.Add(new TourLog(id, datetime, comment, difficulty, totalTime, rating));
            }
            rdr.Close();
            con.Close();

            return tourLogs;
        }

        public void AddNewTour(Tour newTour)
        {
            const string sql = "INSERT INTO tours (t_name, t_description, t_from, t_to, t_transporttype, t_distance, t_time)" +
                               "VALUES (@t_name, @t_description, @t_from, @t_to, @t_transporttype, @t_distance, @t_time)";

            var con = ConOpen();
            var cmd = new NpgsqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@t_name", newTour.Name);
            cmd.Parameters.AddWithValue("@t_description", NpgsqlTypes.NpgsqlDbType.Varchar, (object)newTour.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@t_from", newTour.Start);
            cmd.Parameters.AddWithValue("@t_to", newTour.Destination);
            cmd.Parameters.AddWithValue("@t_transporttype", newTour.TransportType);
            cmd.Parameters.AddWithValue("@t_distance", newTour.Distance);
            cmd.Parameters.AddWithValue("@t_time", newTour.Time.TotalSeconds);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void AddNewTourLog(int tourId, TourLog newTourLog)
        {
            const string sql = "INSERT INTO tourlogs (t_id, tl_datetime, tl_comment, tl_difficulty, tl_totaltime, tl_rating)" +
                               "VALUES (@t_id, @tl_datetime, @tl_comment, @tl_difficulty, @tl_totaltime, @tl_rating)";

            var con = ConOpen();
            var cmd = new NpgsqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@t_id", tourId);
            cmd.Parameters.AddWithValue("@tl_datetime", newTourLog.Datetime);
            cmd.Parameters.AddWithValue("@tl_comment", NpgsqlTypes.NpgsqlDbType.Varchar, (object)newTourLog.Comment ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@tl_difficulty", newTourLog.Difficulty);
            cmd.Parameters.AddWithValue("@tl_totaltime", newTourLog.TotalTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@tl_rating", newTourLog.Rating);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void ModifyTour(int id, Tour newTour)
        {
            const string sql = "UPDATE tours SET " +
                "t_name = @t_name, " +
                "t_description = @t_description, " +
                "t_from = @t_from, " +
                "t_to = @t_to, " +
                "t_transporttype = @t_transporttype, " +
                "t_distance = @t_distance, " +
                "t_time = @t_time " +
                "WHERE t_id = @t_id";

            var con = ConOpen();
            var cmd = new NpgsqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@t_id", id);
            cmd.Parameters.AddWithValue("@t_name", newTour.Name);
            cmd.Parameters.AddWithValue("@t_description", newTour.Description);
            cmd.Parameters.AddWithValue("@t_from", newTour.Start);
            cmd.Parameters.AddWithValue("@t_to", newTour.Destination);
            cmd.Parameters.AddWithValue("@t_transporttype", newTour.TransportType);
            cmd.Parameters.AddWithValue("@t_distance", newTour.Distance);
            cmd.Parameters.AddWithValue("@t_time", newTour.Time.TotalSeconds);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void ModifyTourLog(TourLog tourLog)
        {
            const string sql = "UPDATE tourlogs SET " +
                "tl_datetime = @tl_datetime, " +
                "tl_comment = @tl_comment, " +
                "tl_difficulty = @tl_difficulty, " +
                "tl_totaltime = @tl_totaltime, " +
                "tl_rating = @tl_rating " +
                "WHERE tl_id = @tl_id";

            var con = ConOpen();
            var cmd = new NpgsqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@tl_id", tourLog.Id);
            cmd.Parameters.AddWithValue("@tl_datetime", tourLog.Datetime);
            cmd.Parameters.AddWithValue("@tl_comment", NpgsqlTypes.NpgsqlDbType.Varchar, (object)tourLog.Comment ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@tl_difficulty", tourLog.Difficulty);
            cmd.Parameters.AddWithValue("@tl_totaltime", tourLog.TotalTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@tl_rating", tourLog.Rating);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void DeleteTour(Tour newTour)
        {
            const string sql = "DELETE FROM tours WHERE t_id = @t_id";

            var con = ConOpen();
            var cmd = new NpgsqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@t_id", newTour.Id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void DeleteTourLog(TourLog deleteTourLog)
        {
            const string sql = "DELETE FROM tourlogs WHERE tl_id = @tl_id";

            var con = ConOpen();
            var cmd = new NpgsqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@tl_id", deleteTourLog.Id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public List<Tour> SearchTours(string itemName)
        {
            const string sql = "SELECT DISTINCT tours.* FROM tours LEFT JOIN tourlogs ON (tours.t_id = tourlogs.t_id) WHERE " +
                "LOWER(tours.t_name) LIKE @itemname OR " +
                "LOWER(tours.t_description) LIKE @itemname OR " +
                "LOWER(tours.t_from) LIKE @itemname OR " +
                "LOWER(tours.t_to) LIKE @itemname OR " +
                "LOWER(tourlogs.tl_comment) LIKE @itemname";

            var con = ConOpen();
            var cmd = new NpgsqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@itemname", $"%{itemName}%");
            cmd.Prepare();

            var rdr = cmd.ExecuteReader();

            List<Tour> searchedTours = new List<Tour>();

            while (rdr.Read())
            {
                var id = rdr.GetInt32(0);
                var name = rdr.GetString(1);
                string description;
                if (!rdr.IsDBNull(2))
                    description = rdr.GetString(2);
                else
                    description = string.Empty;
                var from = rdr.GetString(3);
                var to = rdr.GetString(4);
                var transportType = rdr.GetString(5);
                var distance = rdr.GetDouble(6);
                var time = TimeSpan.FromSeconds(rdr.GetInt32(7));

                searchedTours.Add(new Tour(id, name, description, from, to, transportType, distance, time));
            }
            rdr.Close();
            con.Close();

            return searchedTours;
        }

        public int GetCurrentIncrementValue()
        {
            const string LAST_VALUE = "last_value";
            const string TOURS_T_ID_SEQ = "tours_t_id_seq";

            string sql = $"SELECT {LAST_VALUE} FROM {TOURS_T_ID_SEQ}";

            var con = ConOpen();
            var cmd = new NpgsqlCommand(sql, con);
            using var rdr = cmd.ExecuteReader();

            rdr.Read();
            int currentIncrementValue = rdr.GetInt32(0);

            rdr.Close();
            con.Close();
            return currentIncrementValue;
        }
    }
}
