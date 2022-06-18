using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TourPlanner.Models;
using TourPlanner.Utils;
using TourPlanner.BusinessLayer.DictionaryHandler;
using TourPlanner.BusinessLayer.TourHandler;
using TourPlanner.BusinessLayer.PDFGenerator;
using System.Windows;
using TourPlanner.BusinessLayer.Exceptions;
using TourPlanner.BusinessLayer.Logger;

namespace TourPlanner.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ITourHandler TourHandler { get; set; }
        public ITourDictionary TourDictionary { get; set; }
        public ILog4NetLogger Log4NetLogger { get; set; }

        private BaseViewModel selectedViewModel;
        public BaseViewModel SelectedViewModel
        {
            get
            {
                return selectedViewModel;
            }
            set
            {
                selectedViewModel = value;
                RaisePropertyChangedEvent(nameof(SelectedViewModel));
            }
        }

        private string searchName = string.Empty;
        public string SearchName
        {
            get { return searchName; }
            set
            {
                if ((searchName != value))
                {
                    searchName = value;
                    RaisePropertyChangedEvent(nameof(SearchName));
                }
            }
        }

        private string numberOfToursFound;
        public string NumberOfToursFound
        {
            get { return numberOfToursFound; }
            set
            {
                numberOfToursFound = value;
                RaisePropertyChangedEvent(nameof(NumberOfToursFound));
            }
        }

        private Tour currentTour;
        public Tour CurrentTour
        {
            get { return currentTour; }
            set
            {
                if ((currentTour != value) && (value != null))
                {
                    currentTour = value;
                    RaisePropertyChangedEvent(nameof(CurrentTour));
                    SelectedViewModel = new CurrentTourViewModel(this);
                }
            }
        }
        public ObservableCollection<Tour> ToursList { get; private set; }
        public ICommand SearchCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }
        public ICommand AddTourCommand { get; private set; }
        public ICommand GenerateTourSummarizeReportCommand { get; private set; }
        public ICommand SelectEnglishCommand { get; private set; }
        public ICommand SelectGermanCommand { get; private set; }

        public MainViewModel(ITourHandler tourHandler, ITourDictionary tourDictionary, ILog4NetLogger logger)
        {
            TourDictionary = tourDictionary;
            TourHandler = tourHandler;
            Log4NetLogger = logger;

            SelectedViewModel = new WelcomeViewModel(this);
            ToursList = new ObservableCollection<Tour>();

            foreach (Tour item in tourHandler.GetTours())
            {
                item.TransportType = TourDictionary.ChangeTransportTypeToSelectedLanguage(item.TransportType);
                ToursList.Add(item);
            }
            NumberOfToursFound = $"{TourDictionary.GetResourceFromDictionary("StringNumberOfToursFound")} {ToursList.Count}";

            SearchCommand = new RelayCommand(_ =>
            {
                if (string.IsNullOrEmpty(SearchName))
                    return;

                RefreshTourList(tourHandler.SearchForTour(SearchName));
                SelectedViewModel = new WelcomeViewModel(this);
            });

            ClearCommand = new RelayCommand(_ =>
            {
                SearchName = string.Empty;
                RefreshTourList(tourHandler.GetTours());
                SelectedViewModel = new WelcomeViewModel(this);
            });

            AddTourCommand = new RelayCommand(_ =>
            {
                SelectedViewModel = new AddTourViewModel(this);
            });

            GenerateTourSummarizeReportCommand = new RelayCommand(_ =>
            {
                try
                {
                    PDFGenerator.GenerateSummarizedReport(TourHandler, TourDictionary, ToursList);
                    Log4NetLogger.Info(tourDictionary.GetResourceFromDictionary("StringPDFGenerationSuccess"));
                    MessageBox.Show(
                        tourDictionary.GetResourceFromDictionary("StringPDFGenerationSuccess"),
                        tourDictionary.GetResourceFromDictionary("StringTitle"),
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (NoToursException ex)
                {
                    Log4NetLogger.Error(ex.Message);
                    Log4NetLogger.Error(tourDictionary.GetResourceFromDictionary("StringErrorNoTours"));
                    MessageBox.Show(
                        tourDictionary.GetResourceFromDictionary("StringErrorNoTours"),
                        tourDictionary.GetResourceFromDictionary("StringTitle"),
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                catch (PDFGenerationException ex)
                {
                    Log4NetLogger.Error(ex.Message);
                    Log4NetLogger.Error(tourDictionary.GetResourceFromDictionary("StringErrorPDFGenerationError"));
                    MessageBox.Show(
                        tourDictionary.GetResourceFromDictionary("StringErrorPDFGenerationError"),
                        tourDictionary.GetResourceFromDictionary("StringTitle"),
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            });

            SelectEnglishCommand = new RelayCommand(_ =>
            {
                TourDictionary.AddDictionaryToApp("English");
                RefreshTourList(tourHandler.GetTours());
            });

            SelectGermanCommand = new RelayCommand(_ =>
            {
                TourDictionary.AddDictionaryToApp("Deutsch");
                RefreshTourList(tourHandler.GetTours());
            });
        }

        public void RefreshTourList(IEnumerable<Tour> items)
        {
            ToursList.Clear();
            foreach (Tour item in items)
            {
                item.TransportType = TourDictionary.ChangeTransportTypeToSelectedLanguage(item.TransportType);

                if (SelectedViewModel is CurrentTourViewModel)
                {
                    CurrentTourViewModel currentTourViewModel = SelectedViewModel as CurrentTourViewModel;
                    if(currentTourViewModel.CurrentTour.Id == item.Id)
                    {
                        currentTourViewModel.CurrentTour = item;
                    }
                    currentTourViewModel.RefreshTourLogList(TourHandler.GetTourLogs(CurrentTour), this);
                }

                ToursList.Add(item);
            }
            NumberOfToursFound = $"{TourDictionary.GetResourceFromDictionary("StringNumberOfToursFound")} {ToursList.Count}";
        }
    }
}
