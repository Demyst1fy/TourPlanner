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
    public class ModifyTourViewModel : BaseViewModel
    {
        private Visibility isError;
        private string errorText;
        private Tour currentTour;
        public ICommand ModifyCommand { get; set; }
        public ICommand CancelCommand { get; set; }

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

        public ModifyTourViewModel(MainViewModel mainViewModel, ITourHandler tourHandler, ITourDictionary tourDictionary)
        {
            IsError = Visibility.Hidden;
            CurrentTour = mainViewModel.CurrentTour;

            ModifyCommand = new RelayCommand(async _ => {
                if (string.IsNullOrEmpty(CurrentTour.Start) || string.IsNullOrEmpty(CurrentTour.Destination))
                {
                    ErrorText = tourDictionary.GetResourceFromDictionary("StringErrorNotFilled");
                    IsError = Visibility.Visible;
                    return;
                }

                CurrentTour.TransportType = tourDictionary.ChangeTransportTypeToPassBL(CurrentTour.TransportType);

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
                    ErrorText = tourDictionary.GetResourceFromDictionary("StringErrorInvalidValuesResponse");
                    IsError = Visibility.Visible;
                    return;
                }

                Tour modifiedTour = new Tour(CurrentTour.Name, CurrentTour.Description, CurrentTour.Start, CurrentTour.Destination, CurrentTour.TransportType, distance, time);

                tourHandler.ModifyTour(CurrentTour.Id, modifiedTour);

                mainViewModel.RefreshTourList(tourHandler.GetTours());
                mainViewModel.SelectedViewModel = new WelcomeViewModel(mainViewModel, tourHandler, tourDictionary);
            });

            CancelCommand = new RelayCommand(_ => {
                mainViewModel.SelectedViewModel = new CurrentTourViewModel(mainViewModel, tourHandler, tourDictionary);
            });
        }
    }
}
