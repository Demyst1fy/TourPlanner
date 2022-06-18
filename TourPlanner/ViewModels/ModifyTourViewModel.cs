using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer.TourHandler;
using TourPlanner.Models;
using TourPlanner.Utils;
using TourPlanner.BusinessLayer.DictionaryHandler;
using TourPlanner.BusinessLayer.TourAttributes;
using TourPlanner.BusinessLayer.APIRequest;
using TourPlanner.BusinessLayer.Exceptions;

namespace TourPlanner.ViewModels
{
    public class ModifyTourViewModel : BaseViewModel
    {
        private Tour currentTour;
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

        private string errorText = string.Empty;
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

        private bool available;
        public bool Available
        {
            get
            {
                return available;
            }
            set
            {
                available = value;
                RaisePropertyChangedEvent(nameof(Available));
            }
        }

        public ICommand ModifyCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ModifyTourViewModel(MainViewModel mainViewModel, ITourHandler tourHandler, ITourDictionary tourDictionary)
        {
            CurrentTour = mainViewModel.CurrentTour;
            Available = true;

            ModifyCommand = new RelayCommand(async _ => {
                Available = false;
                if (string.IsNullOrEmpty(CurrentTour.Start) || string.IsNullOrEmpty(CurrentTour.Destination))
                {
                    ErrorText = tourDictionary.GetResourceFromDictionary("StringErrorNotFilled");
                    Available = true;
                    return;
                }

                CurrentTour.TransportType = tourDictionary.ChangeTransportTypeToPassBL(CurrentTour.TransportType);

                try
                {
                    TourAPIData tourAPIdata = await MapquestAPIRequest.RequestDirection(CurrentTour.Start, CurrentTour.Destination, CurrentTour.TransportType);
                    int statusCode = tourAPIdata.StatusCode;
                    List<object> messages = tourAPIdata.Message;
                    double distance = tourAPIdata.Distance;
                    TimeSpan time = TimeSpan.FromSeconds(tourAPIdata.Time);

                    Tour modifiedTour = new Tour(CurrentTour.Name, CurrentTour.Description, CurrentTour.Start, CurrentTour.Destination, CurrentTour.TransportType, distance, time);
                    tourHandler.ModifyTour(CurrentTour.Id, modifiedTour);

                    mainViewModel.RefreshTourList(tourHandler.GetTours());
                    mainViewModel.SelectedViewModel = new WelcomeViewModel(mainViewModel, tourHandler, tourDictionary);
                }
                catch (MapquestAPIErrorException ex)
                {
                    ErrorText = $"{nameof(MapquestAPIErrorException)}: {ex.Message} {Environment.NewLine}" +
                    $"Response-Code: {ex.ErrorCode} {Environment.NewLine}" +
                    $"Response-Message: {ex.ErrorMessage}";
                }
                catch (MapquestAPIInvalidValuesException ex)
                {
                    ErrorText = $"{nameof(MapquestAPIInvalidValuesException)}: {ex.Message} {Environment.NewLine}" +
                    $"Values: Distance({ex.InvalidDistance}), Time({TimeSpan.FromSeconds(ex.InvalidTime)} {Environment.NewLine})" +
                    $"{tourDictionary.GetResourceFromDictionary("StringErrorInvalidValuesResponse")}";
                }
                catch (TourAlreadyExistsException ex)
                {
                    ErrorText = $"{tourDictionary.GetResourceFromDictionary("StringErrorTourNameAlreadyExists")}";
                }
                Available = true;
            });

            CancelCommand = new RelayCommand(_ => {
                mainViewModel.SelectedViewModel = new CurrentTourViewModel(mainViewModel, tourHandler, tourDictionary);
            });
        }
    }
}
