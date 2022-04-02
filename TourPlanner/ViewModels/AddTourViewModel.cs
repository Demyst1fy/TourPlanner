using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    public class AddTourViewModel : BaseViewModel
    {
        private ITourHandler tourHandler;
        private string start;
        private string end;
        private string description;
        public ICommand AddCommand { get; set; }

        public string Start
        {
            get { return start; }
            set
            {
                if ((start != value))
                {
                    start = value;
                    RaisePropertyChangedEvent(nameof(Start));
                }
            }
        }

        public string End
        {
            get { return end; }
            set
            {
                if ((end != value))
                {
                    end = value;
                    RaisePropertyChangedEvent(nameof(End));
                }
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                if ((description != value))
                {
                    description = value;
                    RaisePropertyChangedEvent(nameof(Description));
                }
            }
        }

        public AddTourViewModel(MainViewModel mainViewModel)
        {
            this.tourHandler = TourHandler.GetHandler();

            this.AddCommand = new RelayCommand(o => {
                Tour newTour = new Tour(Start, End, Description);
                this.tourHandler.AddNewTour(newTour);
                mainViewModel.Items.Add(newTour);
                mainViewModel.SelectedViewModel = new WelcomeViewModel();
            });
        }
    }
}
