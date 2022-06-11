using System;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.BusinessLayer.JsonClasses;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    public class AddTourViewModel : BaseViewModel
    {
        private ITourHandler tourHandler;
        private Visibility isError;

        private string name;
        private string start;
        private string end;
        private string description;
        private string transportType;
        public ICommand AddCommand { get; set; }

        public string Name
        {
            get { return name; }
            set
            {
                if ((name != value))
                {
                    name = value;
                    RaisePropertyChangedEvent(nameof(Name));
                }
            }
        }

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

        public string TransportType
        {
            get { return transportType; }
            set
            {
                if ((transportType != value))
                {
                    transportType = value;
                    RaisePropertyChangedEvent(nameof(TransportType));
                }
            }
        }
        public Visibility IsError
        {
            get
            {
                return isError;
            }
            set
            {
                isError = value;
                RaisePropertyChangedEvent(nameof(IsError));
            }
        }

        public AddTourViewModel(MainViewModel mainViewModel)
        {
            tourHandler = TourHandler.GetHandler();
            IsError = Visibility.Hidden;

            AddCommand = new RelayCommand(async o => {
                Tour newTour = await tourHandler.GetTourFromAPI(Name, Description, Start, End, TransportType);

                /*if (newTour == null)
                {
                    IsError = Visibility.Visible;
                    return;
                }*/

                tourHandler.AddNewTour(newTour);

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
