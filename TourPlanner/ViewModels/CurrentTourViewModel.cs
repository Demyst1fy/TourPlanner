
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    public class CurrentTourViewModel : BaseViewModel
    {
        private Tour currentTour;

        public Tour CurrentTour
        {
            get
            {
                return currentTour;
            }
            set { currentTour = value; }
        }

        public CurrentTourViewModel(Tour currentTour)
        {
            this.currentTour = currentTour;
        }
    }
}
