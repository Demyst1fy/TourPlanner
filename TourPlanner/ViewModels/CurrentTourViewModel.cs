using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TourPlanner.BusinessLayer.TourHandler;
using TourPlanner.Models;
using TourPlanner.Utils;
using TourPlanner.BusinessLayer.DictionaryHandler;
using TourPlanner.BusinessLayer.TourAttributes;
using TourPlanner.BusinessLayer.PDFGenerator;
using TourPlanner.BusinessLayer.Exceptions;

namespace TourPlanner.ViewModels
{
    public class CurrentTourViewModel : BaseViewModel
    {
        private Tour currentTour;
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

        private TourLog currentTourLog;
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
            }
        }

        private ImageSource? mapImage;
        public ImageSource? MapImage
        {
            get { return mapImage; }
            set
            {
                mapImage = value;
                RaisePropertyChangedEvent(nameof(MapImage));
            }
        }

        private double popularity;
        public double Popularity
        {
            get
            {
                return popularity;
            }
            set
            {
                popularity = value;
                RaisePropertyChangedEvent(nameof(Popularity));
            }
        }

        private double childFriendliness;
        public double ChildFriendliness
        {
            get
            {
                return childFriendliness;
            }
            set
            {
                childFriendliness = value;
                RaisePropertyChangedEvent(nameof(ChildFriendliness));
            }
        }

        private string numberOfTourLogsFound;
        public string NumberOfTourLogsFound
        {
            get { return numberOfTourLogsFound; }
            set
            {
                numberOfTourLogsFound = value;
                RaisePropertyChangedEvent(nameof(NumberOfTourLogsFound));
            }
        }

        public ObservableCollection<TourLog> TourLogsList { get; private set; }

        public ICommand ModifyTourCommand { get; set; }
        public ICommand DeleteTourCommand { get; set; }
        public ICommand GenerateSingleTourReportCommand { get; set; }
        public ICommand AddTourLogCommand { get; set; }
        public ICommand ModifyTourLogCommand { get; set; }
        public ICommand DeleteTourLogCommand { get; set; }


        public CurrentTourViewModel(MainViewModel mainViewModel, ITourHandler tourHandler, ITourDictionary tourDictionary)
        {
            CurrentTour = mainViewModel.CurrentTour;
            MapImage = tourHandler.GetImageFile(CurrentTour);

            TourLogsList = new ObservableCollection<TourLog>();
            foreach (TourLog item in tourHandler.GetTourLogs(CurrentTour))
            {
                item.Difficulty = tourDictionary.ChangeDifficultyToSelectedLanguage(item.Difficulty);
                TourLogsList.Add(item);
            }
            NumberOfTourLogsFound = $"{tourDictionary.GetResourceFromDictionary("StringNumberOfTourLogsFound")} {TourLogsList.Count}";

            Popularity = ComputedTourAttribute.CalculatePopularity(tourHandler, CurrentTour);
            ChildFriendliness = ComputedTourAttribute.CalculateChildFriendliness(tourHandler, tourDictionary, CurrentTour);

            ModifyTourCommand = new RelayCommand(_ => {
                mainViewModel.SelectedViewModel = new ModifyTourViewModel(mainViewModel, tourHandler, tourDictionary);
            });

            DeleteTourCommand = new RelayCommand(_ => {
                MessageBoxResult result = MessageBox.Show(
                    tourDictionary.GetResourceFromDictionary("StringTourDeleteYesNo"),
                    tourDictionary.GetResourceFromDictionary("StringTitle"),
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Question);

                switch (result)
                {
                    case MessageBoxResult.OK:
                        tourHandler.DeleteTour(CurrentTour);
                        mainViewModel.RefreshTourList(tourHandler.GetTours());
                        mainViewModel.SelectedViewModel = new WelcomeViewModel(mainViewModel, tourHandler, tourDictionary);
                        MessageBox.Show(
                            tourDictionary.GetResourceFromDictionary("StringTourDeleted"),
                            tourDictionary.GetResourceFromDictionary("StringTitle"),
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                }
            });

            GenerateSingleTourReportCommand = new RelayCommand(_ => {
                try
                {
                    PDFGenerator.GenerateSingleReport(tourDictionary, CurrentTour, TourLogsList, Popularity, ChildFriendliness);

                    MessageBox.Show(
                        tourDictionary.GetResourceFromDictionary("StringPDFGenerationSuccess"),
                        tourDictionary.GetResourceFromDictionary("StringTitle"),
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch(PDFGenerationException ex)
                {
                    MessageBox.Show(
                        tourDictionary.GetResourceFromDictionary("StringErrorPDFGenerationError"),
                        tourDictionary.GetResourceFromDictionary("StringTitle"),
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            });

            AddTourLogCommand = new RelayCommand(_ => {
                mainViewModel.SelectedViewModel = new AddTourLogViewModel(mainViewModel, tourHandler, tourDictionary);
            });

            ModifyTourLogCommand = new RelayCommand(_ => {
                mainViewModel.SelectedViewModel = new ModifyTourLogViewModel(mainViewModel, this, tourHandler, tourDictionary);
            });

            DeleteTourLogCommand = new RelayCommand(_ => {
                MessageBoxResult result = MessageBox.Show(
                    tourDictionary.GetResourceFromDictionary("StringTourLogDeleteYesNo"),
                    tourDictionary.GetResourceFromDictionary("StringTitle"), 
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Question);

                switch (result)
                {
                    case MessageBoxResult.OK:
                        tourHandler.DeleteTourLog(CurrentTourLog);
                        RefreshTourLogList(tourHandler.GetTourLogs(CurrentTour), tourHandler, tourDictionary);
                        MessageBox.Show(
                            tourDictionary.GetResourceFromDictionary("StringTourLogDeleted"),
                            tourDictionary.GetResourceFromDictionary("StringTitle"),
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                }
            });
        }

        public void RefreshTourLogList(IEnumerable<TourLog> tourLogList, ITourHandler tourHandler, ITourDictionary tourDictionary)
        {
            TourLog? tmpTourLog = null;
            if (CurrentTourLog != null)
            {
                tmpTourLog = new TourLog(CurrentTourLog);
            }

            TourLogsList.Clear();
            foreach (TourLog item in tourLogList)
            {
                item.Difficulty = tourDictionary.ChangeDifficultyToSelectedLanguage(item.Difficulty);

                if(tmpTourLog != null)
                    if (tmpTourLog.Id == item.Id)
                        CurrentTourLog = item;

                TourLogsList.Add(item);
            }

            NumberOfTourLogsFound = $"{tourDictionary.GetResourceFromDictionary("StringNumberOfTourLogsFound")} {TourLogsList.Count}";
            Popularity = ComputedTourAttribute.CalculatePopularity(tourHandler, CurrentTour);
            ChildFriendliness = ComputedTourAttribute.CalculateChildFriendliness(tourHandler, tourDictionary, CurrentTour);
        }
    }
}
