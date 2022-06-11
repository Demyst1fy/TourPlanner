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
            set { currentTour = value;
                RaisePropertyChangedEvent(nameof(CurrentTour)); 
            }
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
            tourHandler = TourHandler.GetHandler();
            CurrentTour = mainViewModel.CurrentTour;
            Image = tourHandler.GetImage(CurrentTour.From, CurrentTour.To);

            ModifyCommand = new RelayCommand(o => {
                mainViewModel.SelectedViewModel = new ModifyTourViewModel(mainViewModel);
            });

            DeleteCommand = new RelayCommand(o => {
                tourHandler.DeleteTour(CurrentTour);
                mainViewModel.Items.Clear();
                foreach (Tour item in tourHandler.GetTours())
                {
                    mainViewModel.Items.Add(item);
                }
                mainViewModel.SelectedViewModel = new WelcomeViewModel();
            });
        }
    }
}
