using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.Utils;

namespace TourPlanner.ViewModels
{
    public class CurrentTourViewModel : BaseViewModel
    {
        private ITourHandler tourHandler;
        private Tour currentTour;
        private TourLog currentTourLog;
        private ImageSource mapImage;
        public ObservableCollection<TourLog> TourLogsList { get; private set; }
        public ICommand AddTourLogCommand { get; set; }
        public ICommand ModifyTourCommand { get; set; }
        public ICommand DeleteTourCommand { get; set; }

        public ICommand ModifyTourLogCommand { get; set; }

        public ICommand DeleteTourLogCommand { get; set; }


        private Visibility isCurrentTourLogSelected;
        public Visibility IsCurrentTourLogSelected
        {
            get
            {
                return isCurrentTourLogSelected;
            }
            set
            {
                isCurrentTourLogSelected = value;
                RaisePropertyChangedEvent(nameof(IsCurrentTourLogSelected));
            }
        }

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

        public TourLog CurrentTourLog
        {
            get
            {
                return currentTourLog;
            }
            set
            {
                currentTourLog = value;
                RaisePropertyChangedEvent(nameof(CurrentTourLog));
                this.IsCurrentTourLogSelected = Visibility.Visible;
            }
        }

        public ImageSource MapImage
        {
            get { return mapImage; }
            set
            {
                mapImage = value;
                RaisePropertyChangedEvent(nameof(MapImage));
            }
        }


        public CurrentTourViewModel(MainViewModel mainViewModel)
        {
            tourHandler = TourHandler.GetHandler();
            CurrentTour = mainViewModel.CurrentTour;
            this.IsCurrentTourLogSelected = Visibility.Hidden;

            MapImage = tourHandler.GetImageFile(CurrentTour);

            TourLogsList = new ObservableCollection<TourLog>();
            foreach (TourLog item in tourHandler.GetTourLogs(CurrentTour))
            {
                TourLogsList.Add(item);
            }

            ModifyTourCommand = new RelayCommand(o => {
                mainViewModel.SelectedViewModel = new ModifyTourViewModel(mainViewModel);
            });

            DeleteTourCommand = new RelayCommand(o => {
                tourHandler.DeleteTour(CurrentTour);

                mainViewModel.RefreshTourList(tourHandler.GetTours());
                mainViewModel.SelectedViewModel = new WelcomeViewModel(mainViewModel);
            });

            AddTourLogCommand = new RelayCommand(o => {
                mainViewModel.SelectedViewModel = new AddTourLogViewModel(mainViewModel);
            });

            ModifyTourLogCommand = new RelayCommand(async o => {
                mainViewModel.SelectedViewModel = new ModifyTourLogViewModel(mainViewModel, this);
            });

            DeleteTourLogCommand = new RelayCommand(o => {
                tourHandler.DeleteTourLog(CurrentTourLog);
                RefreshTourLogList(tourHandler.GetTourLogs(CurrentTour));
                this.IsCurrentTourLogSelected = Visibility.Hidden;
            });
        }

        public void RefreshTourLogList(IEnumerable<TourLog> items)
        {
            TourLogsList.Clear();
            foreach (TourLog item in items)
            {
                TourLogsList.Add(item);
            }
        }
    }
}
