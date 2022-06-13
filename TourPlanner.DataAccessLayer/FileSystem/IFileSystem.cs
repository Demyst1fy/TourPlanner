using System.Windows.Media;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer.FileSystem
{
    public interface IFileSystem
    {
        void SaveImageFile(string start, string end, int currentIncrementValue);
        ImageSource LoadImageFile(Tour tour);
        void DeleteImageFile(Tour deleteTour);
    }
}
