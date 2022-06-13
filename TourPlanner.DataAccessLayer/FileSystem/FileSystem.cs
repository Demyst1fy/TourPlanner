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
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer.FileSystem
{
    public class FileSystem : IFileSystem
    {
        private string _path;
        private static IFileSystem? _fileSystem;

        private FileSystem()
        {
            _path = ConfigurationManager.AppSettings["mapimagespath"];
        }

        public static IFileSystem GetFileSystem()
        {
            if (_fileSystem == null)
            {
                _fileSystem = new FileSystem();
            }
            return _fileSystem;
        }

        public void SaveImageFile(string start, string end, int currentIncrementValue)
        {
            var apikey = ConfigurationManager.AppSettings["mapquestapikey"];

            Directory.CreateDirectory(_path);
            var file = $"{_path}/{currentIncrementValue}.png";

            using WebClient client = new WebClient();
            client.DownloadFile(
                new Uri($"https://www.mapquestapi.com/staticmap/v5/map?" +
                $"start={start}" +
                $"&end={end}" +
                $"&key={apikey}" +
                $"&size=640,480@2x"
                ), file);
        }

        public ImageSource LoadImageFile(Tour tour)
        {
            var filePath = $"{_path}/{tour.Id}.png";

            var bitmap = new BitmapImage();

            using var stream = File.OpenRead(filePath);

            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = stream;
            bitmap.EndInit();
            bitmap.Freeze();

            bitmap.Freeze();
            return bitmap;
        }

        public void DeleteImageFile(Tour deleteTour)
        {
            var directory = ConfigurationManager.AppSettings["mapimagespath"];
            var filePath = $"{directory}/{deleteTour.Id}.png";

            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
