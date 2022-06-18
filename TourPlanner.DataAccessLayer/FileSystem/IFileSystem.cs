using System.Windows.Media;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer.FileSystem
{
    public interface IFileSystem
    {
        void SaveImageFile(Tour tour, int currentIncrementValue);
        ImageSource LoadImageFile(Tour tour);
        void DeleteImageFile(Tour deleteTour);
    }
}
