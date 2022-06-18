using System;
using System.Collections.Generic;
using System.Windows.Media;
using TourPlanner.BusinessLayer.Exceptions;
using TourPlanner.DataAccessLayer.Database;
using TourPlanner.DataAccessLayer.Exceptions;
using TourPlanner.DataAccessLayer.FileSystem;
using TourPlanner.Logger;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer.TourHandler
{
    public class TourHandlerImpl : ITourHandler {

        private IDatabase _database;
        private IFileSystem _fileSystem;
        private ILoggerWrapper _logger;

        public TourHandlerImpl() {
            _database = Database.GetDatabase();
            _fileSystem = FileSystem.GetFileSystem();
            _logger = Log4NetWrapper.CreateLogger("./log4net.config");
        }

        public TourHandlerImpl(IDatabase database, IFileSystem fileSystem)
        {
            _database = database;
            _fileSystem = fileSystem;
        }

        public void AddNewTour(Tour newTour)
        {
            try
            {
                _database.AddNewTour(newTour);
                int currentIncrementValue = _database.GetCurrentIncrementValue();
                _fileSystem.SaveImageFile(newTour, currentIncrementValue);

                _logger.Info($"Tour added: [{newTour.Name}] ID: [{newTour.Id}]");
            } catch (NoMapReceivedException ex)
            {
                _logger.Error($"No Map received: [{ex.Message}]");
            }
            catch (DatabaseException ex)
            {
                _logger.Error($"Database exception: [{ex.Message}]");

                throw new TourAlreadyExistsException(ex.Message);
            }
        }
           

        public void AddNewTourLog(int tourId, TourLog newTourLog)
        {
            try
            {
                _database.AddNewTourLog(tourId, newTourLog);

                _logger.Info($"Tour log added");
            }
            catch (DatabaseException ex)
            {
                _logger.Error(ex.Message);
            }
        }

        public IEnumerable<Tour> GetTours() {
            try
            {
                return _database.GetTours();
            }
            catch (DatabaseException ex)
            {
                _logger.Error(ex.Message);
            }
            return null;
        }

        public IEnumerable<TourLog> GetTourLogs(Tour tour)
        {
            try
            {
                return _database.GetTourLogs(tour);
            }
            catch (DatabaseException ex)
            {
                _logger.Error(ex.Message);
            }
            return null;
        }

        public List<TourLog> GetAllTourLogs()
        {
            try
            {
                return _database.GetAllTourLogs();
            }
            catch (DatabaseException ex)
            {
                _logger.Error(ex.Message);
            }
            return null;
        }

        public ImageSource GetImageFile(Tour tour)
        {
            try
            {
                return _fileSystem.LoadImageFile(tour);
            }
            catch (NoMapImageFileFound ex)
            {
                _logger.Error(ex.Message);
            }
            return null;
        }

        public IEnumerable<Tour> SearchForTour(string searchItem) {
            try
            {
                return _database.SearchTours(searchItem.ToLower());
            }
            catch (DatabaseException ex)
            {
                _logger.Error(ex.Message);
            }
            return null;
        }

        public void ModifyTour(int id, Tour modifiedTour)
        {
            try
            {
                _database.ModifyTour(id, modifiedTour);
                _logger.Info($"Tour modified. ID: [{modifiedTour.Id}] Name: [{modifiedTour.Name}]");
            }
            catch (DatabaseException ex)
            {
                _logger.Error(ex.Message);
                throw new TourAlreadyExistsException($"Tourname: {modifiedTour.Name} already exists.");
            }
        }

        public void ModifyTourLog(TourLog tourLog)
        {
            try
            {
                _database.ModifyTourLog(tourLog);
                _logger.Info($"Tourlog modified. Tour-ID: [{tourLog.TourId}] ID: [{tourLog.Id}]");
            }
            catch (DatabaseException ex)
            {
                _logger.Error(ex.Message);
            }
        }

        public void DeleteTour(Tour deleteTour)
        {
            try
            {
                _fileSystem.DeleteImageFile(deleteTour);
                _database.DeleteTour(deleteTour);
                _logger.Info($"Tour deleted: ID: [{deleteTour.Id}] Name: [{deleteTour.Name}]");
            }
            catch (DatabaseException ex)
            {
                _logger.Error(ex.Message);
            }
        }

        public void DeleteTourLog(TourLog deleteTourLog)
        {
            try
            {
                _database.DeleteTourLog(deleteTourLog);
                _logger.Info($"Tourlog deleted: Tour-ID: [{deleteTourLog.TourId}] ID: [{deleteTourLog.Id}]");
            }
            catch (DatabaseException ex)
            {
                _logger.Error(ex.Message);
            }
        }
    } 
}
