using System;
using System.Collections.Generic;
using System.Configuration;
using TourPlanner.Models;
using Npgsql;
using TourPlanner.DataAccessLayer.Exceptions;

namespace TourPlanner.DataAccessLayer.Database
{
    public class Database : IDatabase
    {

        public string _conString { get; set; }
        private string _dbName;
        private string _dbToursTableName;
        private string _dbTourLogsTableName;
        private static IDatabase? _database;

        private Database()
        {
            _dbName = ConfigurationManager.AppSettings["DatabaseName"] ?? "not found";
            _dbToursTableName = ConfigurationManager.AppSettings["DatabaseToursTableName"] ?? "not found";
            _dbTourLogsTableName = ConfigurationManager.AppSettings["DatabaseTourLogsTableName"] ?? "not found";

            _conString = ConfigurationManager.AppSettings["ConnectionString"] ?? "not found";

            CreateDatabaseIfNotExists();

            _conString += $";Database={_dbName}";

            CreateTablesIfNotExist();
        }

        public static IDatabase GetDatabase()
        {
            if (_database == null)
            {
                _database = new Database();
            }
            return _database;
        }

        private NpgsqlConnection ConOpen()
        {
            var con = new NpgsqlConnection(_conString);
            con.Open();
            return con;
        }

        private void CreateDatabaseIfNotExists()
        {
            string sql = $"SELECT 1 FROM pg_database WHERE datname='{_dbName}'";

            var con = ConOpen();
            var cmd = new NpgsqlCommand(sql, con);
            try
            {
                if (cmd.ExecuteScalar() == null)
                {
                    con.Close();
                    con = ConOpen();

                    string sql2 = $"CREATE DATABASE {_dbName}";

                    cmd = new NpgsqlCommand(sql2, con);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (NpgsqlException ex)
            {
                throw new DatabaseException($"Error in Database occured. Message: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        private void CreateTablesIfNotExist()
        {
            string sql = $"CREATE TABLE IF NOT EXISTS {_dbToursTableName}(" +
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

            try {
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new DatabaseException($"Error in Database occured. Message: {ex.Message}");
            }
            finally
            {
                con.Close();
            }

            string sql2 = $"CREATE TABLE IF NOT EXISTS {_dbTourLogsTableName}(" +
                $"t_id INT NOT NULL CONSTRAINT t_id REFERENCES {_dbToursTableName} ON UPDATE CASCADE ON DELETE CASCADE," +
                "tl_id SERIAL PRIMARY KEY," +
                "tl_datetime TIMESTAMP NOT NULL," +
                "tl_comment VARCHAR(255)," +
                "tl_difficulty VARCHAR(50) NOT NULL," +
                "tl_totaltime INT NOT NULL," +
                "tl_rating INT NOT NULL);";

            con = ConOpen();
            cmd = new NpgsqlCommand(sql2, con);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new DatabaseException($"Error in Database occured. Message: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        public List<Tour> GetTours()
        {
            string sql = $"SELECT * FROM {_dbToursTableName}";
            var con = ConOpen();
            var cmd = new NpgsqlCommand(sql, con);
            var rdr = cmd.ExecuteReader();

            List<Tour> tours = new List<Tour>();
            try
            {
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
            }
            catch (NpgsqlException ex)
            {
                throw new DatabaseException($"Error in Database occured. Message: {ex.Message}");
            }
            finally
            {
                rdr.Close();
                con.Close();
            }

            return tours;
        }

        public List<TourLog> GetTourLogs(Tour tour)
        {
            string sql = $"SELECT * FROM {_dbTourLogsTableName} WHERE t_id = @t_id";

            var con = ConOpen();
            var cmd = new NpgsqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@t_id", tour.Id);
            var rdr = cmd.ExecuteReader();

            List<TourLog> tourLogs = new List<TourLog>();

            try
            {
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
            }
            catch (NpgsqlException ex)
            {
                throw new DatabaseException($"Error in Database occured. Message: {ex.Message}");
            }
            finally
            {
                rdr.Close();
                con.Close();
            }

            return tourLogs;
        }

        public List<TourLog> GetAllTourLogs()
        {
            string sql = $"SELECT * FROM {_dbTourLogsTableName}";

            var con = ConOpen();
            var cmd = new NpgsqlCommand(sql, con);
            var rdr = cmd.ExecuteReader();

            List<TourLog> tourLogs = new List<TourLog>();

            try
            {
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
            }
            catch (NpgsqlException ex)
            {
                throw new DatabaseException($"Error in Database occured. Message: {ex.Message}");
            }
            finally
            {
                rdr.Close();
                con.Close();
            }

            return tourLogs;
        }

        public void AddNewTour(Tour newTour)
        {
            string sql = $"INSERT INTO {_dbToursTableName} (t_name, t_description, t_from, t_to, t_transporttype, t_distance, t_time)" +
                               "VALUES (@t_name, @t_description, @t_from, @t_to, @t_transporttype, @t_distance, @t_time)";

            var con = ConOpen();
            try
            {
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
            }
            catch (NpgsqlException ex)
            {
                throw new DatabaseException($"Error in Database occured. Message: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        public void AddNewTourLog(int tourId, TourLog newTourLog)
        {
            string sql = $"INSERT INTO {_dbTourLogsTableName} (t_id, tl_datetime, tl_comment, tl_difficulty, tl_totaltime, tl_rating)" +
                               "VALUES (@t_id, @tl_datetime, @tl_comment, @tl_difficulty, @tl_totaltime, @tl_rating)";

            var con = ConOpen();

            try
            {
                var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@t_id", tourId);
                cmd.Parameters.AddWithValue("@tl_datetime", newTourLog.Datetime);
                cmd.Parameters.AddWithValue("@tl_comment", NpgsqlTypes.NpgsqlDbType.Varchar, (object)newTourLog.Comment ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@tl_difficulty", newTourLog.Difficulty);
                cmd.Parameters.AddWithValue("@tl_totaltime", newTourLog.TotalTime.TotalSeconds);
                cmd.Parameters.AddWithValue("@tl_rating", newTourLog.Rating);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new DatabaseException($"Error in Database occured. Message: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        public void ModifyTour(int id, Tour newTour)
        {
            string sql = $"UPDATE {_dbToursTableName} SET " +
                "t_name = @t_name, " +
                "t_description = @t_description, " +
                "t_from = @t_from, " +
                "t_to = @t_to, " +
                "t_transporttype = @t_transporttype, " +
                "t_distance = @t_distance, " +
                "t_time = @t_time " +
                "WHERE t_id = @t_id";

            var con = ConOpen();

            try
            {
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
            }
            catch (NpgsqlException ex)
            {
                throw new DatabaseException($"Error in Database occured. Message: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        public void ModifyTourLog(TourLog tourLog)
        {
            string sql = $"UPDATE {_dbTourLogsTableName} SET " +
                "tl_datetime = @tl_datetime, " +
                "tl_comment = @tl_comment, " +
                "tl_difficulty = @tl_difficulty, " +
                "tl_totaltime = @tl_totaltime, " +
                "tl_rating = @tl_rating " +
                "WHERE tl_id = @tl_id";

            var con = ConOpen();

            try
            {
                var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@tl_id", tourLog.Id);
                cmd.Parameters.AddWithValue("@tl_datetime", tourLog.Datetime);
                cmd.Parameters.AddWithValue("@tl_comment", NpgsqlTypes.NpgsqlDbType.Varchar, (object)tourLog.Comment ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@tl_difficulty", tourLog.Difficulty);
                cmd.Parameters.AddWithValue("@tl_totaltime", tourLog.TotalTime.TotalSeconds);
                cmd.Parameters.AddWithValue("@tl_rating", tourLog.Rating);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new DatabaseException($"Error in Database occured. Message: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        public void DeleteTour(Tour deleteTour)
        {
            string sql = $"DELETE FROM {_dbToursTableName} WHERE t_id = @t_id";

            var con = ConOpen();

            try
            {
                var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@t_id", deleteTour.Id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new DatabaseException($"Error in Database occured. Message: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        public void DeleteTourLog(TourLog deleteTourLog)
        {
            string sql = $"DELETE FROM {_dbTourLogsTableName} WHERE tl_id = @tl_id";

            var con = ConOpen();

            try
            {
                var cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@tl_id", deleteTourLog.Id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                throw new DatabaseException($"Error in Database occured. Message: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        public List<Tour> SearchTours(string itemName)
        {
            string sql = $"SELECT DISTINCT {_dbToursTableName}.* FROM {_dbToursTableName} LEFT JOIN {_dbTourLogsTableName} ON ({_dbToursTableName}.t_id = {_dbTourLogsTableName}.t_id) WHERE " +
                $"LOWER({_dbToursTableName}.t_name) LIKE @itemname OR " +
                $"LOWER({_dbToursTableName}.t_description) LIKE @itemname OR " +
                $"LOWER({_dbToursTableName}.t_from) LIKE @itemname OR " +
                $"LOWER({_dbToursTableName}.t_to) LIKE @itemname OR " +
                $"LOWER({_dbTourLogsTableName}.tl_comment) LIKE @itemname";

            var con = ConOpen();

            var cmd = new NpgsqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@itemname", $"%{itemName}%");
            cmd.Prepare();

            var rdr = cmd.ExecuteReader();
            List<Tour> searchedTours = new List<Tour>();

            try
            {
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
            }
            catch (NpgsqlException ex)
            {
                throw new DatabaseException($"Error in Database occured. Message: {ex.Message}");
            }
            finally
            {
                rdr.Close();
                con.Close();
            }

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
