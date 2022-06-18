using System;
using System.Collections.Generic;
using System.Windows.Media;
using TourPlanner.BusinessLayer.Exceptions;
using TourPlanner.DataAccessLayer.Database;
using TourPlanner.DataAccessLayer.Exceptions;
using TourPlanner.DataAccessLayer.FileSystem;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer.TourHandler
{
    public class TourHandlerImpl : ITourHandler {

        private IDatabase _database;
        private IFileSystem _fileSystem;

        public TourHandlerImpl() {
            _database = Database.GetDatabase();
            _fileSystem = FileSystem.GetFileSystem();
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

                // log
            } catch (NoMapReceivedException ex)
            {
                // log
            }
            catch (DatabaseException ex)
            {
                // log
                throw new TourAlreadyExistsException(ex.Message);
            }
        }
           

        public void AddNewTourLog(int tourId, TourLog newTourLog)
        {
            try
            {
                _database.AddNewTourLog(tourId, newTourLog);

                // log
            }
            catch (DatabaseException ex)
            {
                // log
            }
        }

        public IEnumerable<Tour> GetTours() {
            try
            {
                return _database.GetTours();
            }
            catch (DatabaseException ex)
            {
                // log
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
                // log
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
                // log
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
                // log
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
                // log
            }
            return null;
        }

        public void ModifyTour(int id, Tour modifiedTour)
        {
            try
            {
                _database.ModifyTour(id, modifiedTour);
                // log
            }
            catch (DatabaseException ex)
            {
                // log
                throw new TourAlreadyExistsException($"Tourname: {modifiedTour.Name} already exists.");
            }
        }

        public void ModifyTourLog(TourLog tourLog)
        {
            try
            {
                _database.ModifyTourLog(tourLog);
                // log
            }
            catch (DatabaseException ex)
            {
                // log
            }
        }

        public void DeleteTour(Tour deleteTour)
        {
            try
            {
                _fileSystem.DeleteImageFile(deleteTour);
                _database.DeleteTour(deleteTour);
                // log
            }
            catch (DatabaseException ex)
            {
                // log
            }
        }

        public void DeleteTourLog(TourLog deleteTourLog)
        {
            try
            {
                _database.DeleteTourLog(deleteTourLog);
                // log
            }
            catch (DatabaseException ex)
            {
                // log
            }
        }
    } 
}
