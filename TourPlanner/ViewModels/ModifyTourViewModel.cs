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
    public class ModifyTourViewModel : BaseViewModel
    {
        private ITourHandler tourHandler;
        private Visibility isError;
        private string errorText;
        private Tour currentTour;
        public ICommand ModifyCommand { get; set; }

        public Tour CurrentTour
        {
            get
            {
                return currentTour;
            }
            set
            {
                currentTour = value;
                RaisePropertyChangedEvent(nameof(CurrentTour));
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

        public ModifyTourViewModel(MainViewModel mainViewModel)
        {
            tourHandler = TourHandler.GetHandler();
            IsError = Visibility.Hidden;
            CurrentTour = mainViewModel.CurrentTour;

            ModifyCommand = new RelayCommand(async o => {
                if (string.IsNullOrEmpty(CurrentTour.Start) || string.IsNullOrEmpty(CurrentTour.Destination))
                {
                    ErrorText = ErrorText = (string)Application.Current.Resources["StringErrorNotFilled"];
                    IsError = Visibility.Visible;
                    return;
                }

                ChangeTransportTypeToPassBL();

                TourAPIData tourAPIdata = await APIRequest.RequestDirection(CurrentTour.Start, CurrentTour.Destination, CurrentTour.TransportType);

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
                    ErrorText = ErrorText = (string)Application.Current.Resources["StringErrorInvalidValuesResponse"];
                    IsError = Visibility.Visible;
                    return;
                }

                Tour modifiedTour = new Tour(CurrentTour.Name, CurrentTour.Description, CurrentTour.Start, CurrentTour.Destination, CurrentTour.TransportType, distance, time);

                this.tourHandler.ModifyTour(CurrentTour.Id, modifiedTour);

                mainViewModel.RefreshTourList(tourHandler.GetTours());
                mainViewModel.SelectedViewModel = new WelcomeViewModel(mainViewModel);
            });
        }

        public void ChangeTransportTypeToPassBL()
        {
            if (CurrentTour.TransportType == (string)Application.Current.Resources["StringTourCar"])
                CurrentTour.TransportType = "Car";
            else if (CurrentTour.TransportType == (string)Application.Current.Resources["StringTourFoot"])
                CurrentTour.TransportType = "Foot";
            else if (CurrentTour.TransportType == (string)Application.Current.Resources["StringTourBicycle"])
                CurrentTour.TransportType = "Bicycle";
            else
                CurrentTour.TransportType = "Car";
        }
    }
}
