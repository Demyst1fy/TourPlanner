
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
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

        public ICommand DeleteTour { get; set; }

        private ITourHandler tourHandler;

        public CurrentTourViewModel(Tour currentTour, ObservableCollection<Tour> items)
        {
            this.currentTour = currentTour;
            this.tourHandler = TourHandlerSingleton.GetHandler();

            this.DeleteTour = new RelayCommand(o => {
                this.tourHandler.DeleteTour(CurrentTour);
                items.Remove(CurrentTour);
            });
        }
    }
}
