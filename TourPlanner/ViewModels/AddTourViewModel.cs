using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.BusinessLayer.JsonClasses;
using TourPlanner.Models;
using TourPlanner.Utils;
using TourPlanner.DictionaryHandler;

namespace TourPlanner.ViewModels
{
    public class AddTourViewModel : BaseViewModel
    {
        private Visibility isError;
        private string errorText;

        private string name;
        private string start;
        private string destination;
        private string description;
        private string transportType;
        public ICommand AddCommand { get; set; }
        public ICommand CancelCommand { get; set; }

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

        public AddTourViewModel(MainViewModel mainViewModel, ITourHandler tourHandler, ITourDictionary tourDictionary)
        {
            IsError = Visibility.Hidden;

            TransportType = tourDictionary.GetResourceFromDictionary("StringTourCar");

            AddCommand = new RelayCommand(async _ =>
            {
                if (string.IsNullOrEmpty(Start) || string.IsNullOrEmpty(Destination))
                {
                    ErrorText = tourDictionary.GetResourceFromDictionary("StringErrorNotFilled");
                    IsError = Visibility.Visible;
                    return;
                }

                TransportType = tourDictionary.ChangeTransportTypeToPassBL(TransportType);

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
                    ErrorText = tourDictionary.GetResourceFromDictionary("StringErrorInvalidValuesResponse");
                    IsError = Visibility.Visible;
                    return;
                }

                Tour newTour = new Tour(Name, Description, Start, Destination, TransportType, distance, time);

                tourHandler.AddNewTour(newTour);

                mainViewModel.RefreshTourList(tourHandler.GetTours());
                mainViewModel.SelectedViewModel = new WelcomeViewModel(mainViewModel, tourHandler, tourDictionary);
            });

            CancelCommand = new RelayCommand(_ => {
                mainViewModel.SelectedViewModel = new WelcomeViewModel(mainViewModel, tourHandler, tourDictionary);
            });
        }
    }
}
