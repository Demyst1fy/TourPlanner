using System;
using System.Collections.Generic;
using System.Windows.Media;
using TourPlanner.BusinessLayer.Exceptions;
using TourPlanner.BusinessLayer.Logger;
using TourPlanner.DataAccessLayer.Database;
using TourPlanner.DataAccessLayer.Exceptions;
using TourPlanner.DataAccessLayer.FileSystem;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer.TourHandler
{
    public class TourHandlerImpl : ITourHandler {
        private static ITourHandler tourHandler;

        private IDatabase _database;
        private IFileSystem _fileSystem;
        private ILog4NetLogger _logger;

        private TourHandlerImpl() {
            _database = DatabaseRepository.GetDatabase();
            _fileSystem = FileSystem.GetFileSystem();
            _logger = Log4NetLoggerFactory.GetLogger();
        }

        private TourHandlerImpl(IDatabase database, IFileSystem fileSystem, ILog4NetLogger logger)
        {
            _database = database;
            _fileSystem = fileSystem;
            _logger = logger;
        }

        public static ITourHandler CreateHandler()
        {
            if (tourHandler == null)
            {
                tourHandler = new TourHandlerImpl();
            }
            return tourHandler;
        }

        public static ITourHandler CreateHandler(IDatabase database, IFileSystem fileSystem, ILog4NetLogger logger)
        {
            if (tourHandler == null)
            {
                tourHandler = new TourHandlerImpl(database, fileSystem, logger);
            }
            return tourHandler;
        }

        public void AddNewTour(Tour newTour)
        {
            try
            {
                _database.AddNewTour(newTour);
                int currentIncrementValue = _database.GetCurrentIncrementValue();
                _fileSystem.SaveImageFile(newTour, currentIncrementValue);

                _logger.Info($"Tour added: [{newTour.Name}]");
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
                _logger.Error($"Database exception: [{ex.Message}]");
            }
        }

        public IEnumerable<Tour> GetTours() {
            try
            {
                return _database.GetTours();
            }
            catch (DatabaseException ex)
            {
                _logger.Error($"Database exception: [{ex.Message}]");
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
                _logger.Error($"Database exception: [{ex.Message}]");
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
                _logger.Error($"Database exception: [{ex.Message}]");
            }
            return null;
        }

        public ImageSource GetImageFile(Tour tour)
        {
            try
            {
                return _fileSystem.LoadImageFile(tour);
            }
            catch (NoMapImageFileFoundException ex)
            {
                _logger.Error($"No map image file found: [{ex.Message}]");
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
                _logger.Error($"Database exception: [{ex.Message}]");
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
                _logger.Error($"Database exception: [{ex.Message}]");
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
                _logger.Error($"Database exception: [{ex.Message}]");
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
                _logger.Error($"Database exception: [{ex.Message}]");
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
                _logger.Error($"Database exception: [{ex.Message}]");
            }
        }
    } 
}
