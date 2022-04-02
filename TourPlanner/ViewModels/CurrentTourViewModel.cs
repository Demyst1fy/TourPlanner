using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    public class CurrentTourViewModel : BaseViewModel
    {
        private ITourHandler tourHandler;
        private Tour currentTour;
        private string image;
        public ICommand ModifyCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public Tour CurrentTour
        {
            get
            {
                return currentTour;
            }
            set { currentTour = value; }
        }

        public string Image
        {
            get { return image; }
            set
            {
                image = value;
                RaisePropertyChangedEvent(nameof(Image));
            }
        }

        public CurrentTourViewModel(MainViewModel mainViewModel)
        {
            this.tourHandler = TourHandler.GetHandler();
            this.currentTour = mainViewModel.CurrentTour;
            this.image = this.tourHandler.GetImage(this.currentTour.Start, this.currentTour.End);

            this.DeleteCommand = new RelayCommand(o => {
                this.tourHandler.DeleteTour(CurrentTour);
                mainViewModel.Items.Remove(CurrentTour);
                mainViewModel.SelectedViewModel = new WelcomeViewModel();
            });
        }
    }
}
