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
using TourPlanner.BusinessLayer.ExportImport;

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
        public ICommand ExportTourCommand { get; set; }
        public ICommand AddTourLogCommand { get; set; }
        public ICommand ModifyTourLogCommand { get; set; }
        public ICommand DeleteTourLogCommand { get; set; }


        public CurrentTourViewModel(MainViewModel mainViewModel)
        {
            CurrentTour = mainViewModel.CurrentTour;
            MapImage = mainViewModel.TourHandler.GetImageFile(CurrentTour);

            TourLogsList = new ObservableCollection<TourLog>();
            foreach (TourLog item in mainViewModel.TourHandler.GetTourLogs(CurrentTour))
            {
                item.Difficulty = mainViewModel.TourDictionary.ChangeDifficultyToSelectedLanguage(item.Difficulty);
                TourLogsList.Add(item);
            }
            NumberOfTourLogsFound = $"{mainViewModel.TourDictionary.GetResourceFromDictionary("StringNumberOfTourLogsFound")} {TourLogsList.Count}";

            Popularity = ComputedTourAttribute.CalculatePopularity(mainViewModel.TourHandler, CurrentTour);
            ChildFriendliness = ComputedTourAttribute.CalculateChildFriendliness(mainViewModel.TourHandler, mainViewModel.TourDictionary, CurrentTour);

            ModifyTourCommand = new RelayCommand(_ => {
                mainViewModel.SelectedViewModel = new ModifyTourViewModel(mainViewModel);
            });

            DeleteTourCommand = new RelayCommand(_ => {
                MessageBoxResult result = MessageBox.Show(
                    mainViewModel.TourDictionary.GetResourceFromDictionary("StringTourDeleteYesNo"),
                    mainViewModel.TourDictionary.GetResourceFromDictionary("StringTitle"),
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Question);

                switch (result)
                {
                    case MessageBoxResult.OK:
                        mainViewModel.TourHandler.DeleteTour(CurrentTour);
                        mainViewModel.RefreshTourList(mainViewModel.TourHandler.GetTours());
                        mainViewModel.SelectedViewModel = new WelcomeViewModel(mainViewModel);
                        MessageBox.Show(
                            mainViewModel.TourDictionary.GetResourceFromDictionary("StringTourDeleted"),
                            mainViewModel.TourDictionary.GetResourceFromDictionary("StringTitle"),
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
                    PDFGenerator.GenerateSingleReport(mainViewModel.TourDictionary, CurrentTour, TourLogsList, Popularity, ChildFriendliness);

                    mainViewModel.Log4NetLogger.Info(mainViewModel.TourDictionary.GetResourceFromDictionary("StringPDFGenerationSuccess"));
                    MessageBox.Show(
                        mainViewModel.TourDictionary.GetResourceFromDictionary("StringPDFGenerationSuccess"),
                        mainViewModel.TourDictionary.GetResourceFromDictionary("StringTitle"),
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch(PDFGenerationException ex)
                {
                    mainViewModel.Log4NetLogger.Error(ex.Message);
                    mainViewModel.Log4NetLogger.Error(mainViewModel.TourDictionary.GetResourceFromDictionary("StringErrorPDFGenerationError"));
                    MessageBox.Show(
                        mainViewModel.TourDictionary.GetResourceFromDictionary("StringErrorPDFGenerationError"),
                        mainViewModel.TourDictionary.GetResourceFromDictionary("StringTitle"),
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            });

            AddTourLogCommand = new RelayCommand(_ => {
                mainViewModel.SelectedViewModel = new AddTourLogViewModel(mainViewModel);
            });

            ExportTourCommand = new RelayCommand(_ => {
                string message = string.Empty;
                MessageBoxImage messageBoxImage = MessageBoxImage.Information;

                bool result = JsonFileHandler.ExportTour(CurrentTour);

                if (result)
                {
                    message = mainViewModel.TourDictionary.GetResourceFromDictionary("StringExportTourSuccess");
                    messageBoxImage = MessageBoxImage.Information;
                }
                else
                {
                    message = mainViewModel.TourDictionary.GetResourceFromDictionary("StringErrorFileExport");
                    messageBoxImage = MessageBoxImage.Error;
                }

                MessageBox.Show(
                        message,
                        mainViewModel.TourDictionary.GetResourceFromDictionary("StringTitle"),
                        MessageBoxButton.OK,
                        messageBoxImage);
            });

            ModifyTourLogCommand = new RelayCommand(_ => {
                mainViewModel.SelectedViewModel = new ModifyTourLogViewModel(mainViewModel, this);
            });

            DeleteTourLogCommand = new RelayCommand(_ => {
                MessageBoxResult result = MessageBox.Show(
                    mainViewModel.TourDictionary.GetResourceFromDictionary("StringTourLogDeleteYesNo"),
                    mainViewModel.TourDictionary.GetResourceFromDictionary("StringTitle"), 
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Question);

                switch (result)
                {
                    case MessageBoxResult.OK:
                        mainViewModel.TourHandler.DeleteTourLog(CurrentTourLog);
                        RefreshTourLogList(mainViewModel.TourHandler.GetTourLogs(CurrentTour), mainViewModel);
                        MessageBox.Show(
                            mainViewModel.TourDictionary.GetResourceFromDictionary("StringTourLogDeleted"),
                            mainViewModel.TourDictionary.GetResourceFromDictionary("StringTitle"),
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                }
            });
        }

        public void RefreshTourLogList(IEnumerable<TourLog> tourLogList, MainViewModel mainViewModel)
        {
            TourLog? tmpTourLog = null;
            if (CurrentTourLog != null)
            {
                tmpTourLog = new TourLog(CurrentTourLog);
            }

            TourLogsList.Clear();
            foreach (TourLog item in tourLogList)
            {
                item.Difficulty = mainViewModel.TourDictionary.ChangeDifficultyToSelectedLanguage(item.Difficulty);

                if(tmpTourLog != null)
                    if (tmpTourLog.Id == item.Id)
                        CurrentTourLog = item;

                TourLogsList.Add(item);
            }

            NumberOfTourLogsFound = $"{mainViewModel.TourDictionary.GetResourceFromDictionary("StringNumberOfTourLogsFound")} {TourLogsList.Count}";
            Popularity = ComputedTourAttribute.CalculatePopularity(mainViewModel.TourHandler, CurrentTour);
            ChildFriendliness = ComputedTourAttribute.CalculateChildFriendliness(mainViewModel.TourHandler, mainViewModel.TourDictionary, CurrentTour);
        }
    }
}
