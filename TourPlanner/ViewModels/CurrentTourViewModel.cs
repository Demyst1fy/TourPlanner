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
            this.currentTour = mainViewModel.CurrentTour;
            this.tourHandler = TourHandler.GetHandler();
            this.image = $"https://www.mapquestapi.com/staticmap/v5/map?start={this.currentTour.Start}&end={this.currentTour.End}&key=P6T1ueQLrFgHyNoeG6ewTuebM6uHxMPa";

            this.DeleteCommand = new RelayCommand(o => {
                this.tourHandler.DeleteTour(CurrentTour);
                mainViewModel.Items.Remove(CurrentTour);
                mainViewModel.SelectedViewModel = new WelcomeViewModel();
            });
        }
    }
}
