using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TourPlanner.DataAccessLayer.Exceptions;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer.FileSystem
{
    public class FileSystem : IFileSystem
    {
        private string _path;
        private static IFileSystem? _fileSystem;

        private FileSystem()
        {
            _path = ConfigurationManager.AppSettings["MapImagesPath"];
        }

        public static IFileSystem GetFileSystem()
        {
            if (_fileSystem == null)
            {
                _fileSystem = new FileSystem();
            }
            return _fileSystem;
        }

        public void SaveImageFile(Tour tour, int currentIncrementValue)
        {
            var apiKey = ConfigurationManager.AppSettings["MapquestAPIKey"];
            var mapQuestAPIDirectionLink = ConfigurationManager.AppSettings["MapQuestAPIStaticMap"];

            Directory.CreateDirectory(_path);
            var file = $"{_path}/{currentIncrementValue}.png";

            try
            {
                using WebClient client = new WebClient();
                client.DownloadFile(
                    new Uri($"{mapQuestAPIDirectionLink}" +
                    $"start={tour.Start}" +
                    $"&end={tour.Destination}" +
                    $"&key={apiKey}" +
                    $"&size=640,480@2x"
                    ), file);
            } catch (Exception ex)
            {
                throw new NoMapReceivedException($"No map received for tour: {tour.Name} {Environment.NewLine} Message: {ex.Message}");
            }
        }

        public ImageSource LoadImageFile(Tour tour)
        {
            var filePath = $"{_path}/{tour.Id}.png";

            var bitmap = new BitmapImage();

            try { 
                using var stream = File.OpenRead(filePath);
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                bitmap.Freeze();
            } catch (Exception ex)
            {
                throw new NoMapImageFileFound($"No map image file found for tour: {tour.Name} {Environment.NewLine} Message: {ex.Message}");
            }

            return bitmap;
        }

        public void DeleteImageFile(Tour deleteTour)
        {
            var filePath = $"{_path}/{deleteTour.Id}.png";

            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
