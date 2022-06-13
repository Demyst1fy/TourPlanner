using System;
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
                Tour modifiedTour = await tourHandler.GetTourFromAPI(CurrentTour.Name, CurrentTour.Description, CurrentTour.From, CurrentTour.To, CurrentTour.TransportType);

                /*if (newTour == null)
                {
                    IsError = Visibility.Visible;
                    return;
                }*/

                this.tourHandler.ModifyTour(CurrentTour.Id, modifiedTour);

                mainViewModel.RefreshTourList(tourHandler.GetTours());
                mainViewModel.SelectedViewModel = new WelcomeViewModel(mainViewModel);
            });
        }
    }
}
