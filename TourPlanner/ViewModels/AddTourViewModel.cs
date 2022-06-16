using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.BusinessLayer.JsonClasses;
using TourPlanner.Models;
using TourPlanner.Utils;

namespace TourPlanner.ViewModels
{
    public class AddTourViewModel : BaseViewModel
    {
        private ITourHandler tourHandler;
        private Visibility isError;
        private string errorText;

        private string name;
        private string start;
        private string destination;
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

        public string Destination
        {
            get { return destination; }
            set
            {
                if ((destination != value))
                {
                    destination = value;
                    RaisePropertyChangedEvent(nameof(Destination));
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

        public string ErrorText
        {
            get
            {
                return errorText;
            }
            set
            {
                errorText = value;
                RaisePropertyChangedEvent(nameof(ErrorText));
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
            TransportType = (string)Application.Current.Resources["StringTourCar"];

            AddCommand = new RelayCommand(async o => {
                if(string.IsNullOrEmpty(Start) || string.IsNullOrEmpty(Destination))
                {
                    ErrorText = (string)Application.Current.Resources["StringErrorNotFilled"];
                    IsError = Visibility.Visible;
                    return;
                }

                ChangeTransportTypeToPassBL();

                TourAPIData tourAPIdata = await APIRequest.RequestDirection(Start, Destination, TransportType);

                int statusCode = tourAPIdata.StatusCode;
                List<object> messages = tourAPIdata.Message;
                double distance = tourAPIdata.Distance;
                TimeSpan time = TimeSpan.FromSeconds(tourAPIdata.Time);

                if (statusCode != 0 || messages.Count != 0)
                {
                    ErrorText = $"Response: {statusCode} {Environment.NewLine}Message: {messages[0]}";
                    IsError = Visibility.Visible;
                    return;
                }

                if (distance == 0.0 || time == TimeSpan.Parse("00:00:00"))
                {
                    ErrorText = (string)Application.Current.Resources["StringErrorInvalidValuesResponse"];
                    IsError = Visibility.Visible;
                    return;
                }

                Tour newTour = new Tour(Name, Description, Start, Destination, TransportType, distance, time);

                tourHandler.AddNewTour(newTour);

                mainViewModel.RefreshTourList(tourHandler.GetTours());
                mainViewModel.SelectedViewModel = new WelcomeViewModel(mainViewModel);
            });
        }
        public void ChangeTransportTypeToPassBL()
        {
            if (TransportType == (string)Application.Current.Resources["StringTourCar"])
                TransportType = "Car";
            else if (TransportType == (string)Application.Current.Resources["StringTourFoot"])
                TransportType = "Foot";
            else if (TransportType == (string)Application.Current.Resources["StringTourBicycle"])
                TransportType = "Bicycle";
            else
                TransportType = "Car";
        }
    }
}
