using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TourPlanner.Models;
using TourPlanner.Utils;
using TourPlanner.BusinessLayer.DictionaryHandler;
using TourPlanner.BusinessLayer.TourHandler;

namespace TourPlanner.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ITourHandler _tourHandler;
        private ITourDictionary _tourDictionary;

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
                    SelectedViewModel = new CurrentTourViewModel(this, _tourHandler, _tourDictionary);
                }
            }
        }
        public ObservableCollection<Tour> ToursList { get; private set; }
        public ICommand SearchCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }
        public ICommand AddTourCommand { get; private set; }
        public ICommand SelectEnglishCommand { get; private set; }
        public ICommand SelectGermanCommand { get; private set; }

        public MainViewModel(ITourHandler tourHandler, ITourDictionary tourDictionary)
        {
            _tourDictionary = tourDictionary;
            _tourHandler = tourHandler;
            SelectedViewModel = new WelcomeViewModel(this, _tourHandler, _tourDictionary);
            ToursList = new ObservableCollection<Tour>();

            foreach (Tour item in tourHandler.GetTours())
            {
                item.TransportType = _tourDictionary.ChangeTransportTypeToSelectedLanguage(item.TransportType);
                ToursList.Add(item);
            }
            NumberOfToursFound = $"{_tourDictionary.GetResourceFromDictionary("StringNumberOfToursFound")} {ToursList.Count}";

            SearchCommand = new RelayCommand(_ =>
            {
                if (string.IsNullOrEmpty(SearchName))
                    return;

                RefreshTourList(tourHandler.SearchForTour(SearchName));
                SelectedViewModel = new WelcomeViewModel(this, _tourHandler, _tourDictionary);
            });

            ClearCommand = new RelayCommand(_ =>
            {
                SearchName = string.Empty;
                RefreshTourList(tourHandler.GetTours());
                SelectedViewModel = new WelcomeViewModel(this, _tourHandler, _tourDictionary);
            });

            AddTourCommand = new RelayCommand(_ =>
            {
                SelectedViewModel = new AddTourViewModel(this, _tourHandler, tourDictionary);
            });

            SelectEnglishCommand = new RelayCommand(_ =>
            {
                _tourDictionary.AddDictionaryToApp("English");
                RefreshTourList(tourHandler.GetTours());
            });

            SelectGermanCommand = new RelayCommand(_ =>
            {
                _tourDictionary.AddDictionaryToApp("Deutsch");
                RefreshTourList(tourHandler.GetTours());
            });
        }

        public void RefreshTourList(IEnumerable<Tour> items)
        {
            ToursList.Clear();
            foreach (Tour item in items)
            {
                item.TransportType = _tourDictionary.ChangeTransportTypeToSelectedLanguage(item.TransportType);

                if (SelectedViewModel is CurrentTourViewModel)
                {
                    CurrentTourViewModel currentTourViewModel = SelectedViewModel as CurrentTourViewModel;
                    if(currentTourViewModel.CurrentTour.Id == item.Id)
                    {
                        currentTourViewModel.CurrentTour = item;
                    }
                    currentTourViewModel.RefreshTourLogList(_tourHandler.GetTourLogs(CurrentTour), _tourHandler, _tourDictionary);
                }

                ToursList.Add(item);
            }
            NumberOfToursFound = $"{_tourDictionary.GetResourceFromDictionary("StringNumberOfToursFound")} {ToursList.Count}";
        }
    }
}
