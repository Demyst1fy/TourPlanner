using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.Utils;
using TourPlanner.DictionaryHandler;

namespace TourPlanner.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ITourHandler _tourHandler;
        private ITourDictionary _tourDictionary;
        private BaseViewModel selectedViewModel;
        private string searchName;
        private Tour currentTour;
        public ObservableCollection<Tour> ToursList { get; private set; }
        public ICommand SearchCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }
        public ICommand AddTourCommand { get; private set; }
        public ICommand SelectEnglishCommand { get; private set; }
        public ICommand SelectGermanCommand { get; private set; }

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

        public MainViewModel(ITourHandler tourHandler, ITourDictionary tourDictionary)
        {
            this._tourDictionary = tourDictionary;
            _tourHandler = tourHandler;

            SelectedViewModel = new WelcomeViewModel(this, _tourHandler, _tourDictionary);

            ToursList = new ObservableCollection<Tour>();

            foreach (Tour item in tourHandler.GetTours())
            {
                item.TransportType = _tourDictionary.ChangeTransportTypeToSelectedLanguage(item.TransportType);

                ToursList.Add(item);
            }

            SearchCommand = new RelayCommand(_ =>
            {
                if (string.IsNullOrEmpty(SearchName))
                    return;

                RefreshTourList(tourHandler.SearchForTour(SearchName));
                SelectedViewModel = new WelcomeViewModel(this, _tourHandler, _tourDictionary);
            });

            ClearCommand = new RelayCommand(_ =>
            {
                SearchName = "";
                RefreshTourList(tourHandler.GetTours());
                SelectedViewModel = new WelcomeViewModel(this, _tourHandler, _tourDictionary);
            });

            AddTourCommand = new RelayCommand(_ =>
            {
                SelectedViewModel = new AddTourViewModel(this, tourHandler, tourDictionary);
            });

            SelectEnglishCommand = new RelayCommand(_ =>
            {
                _tourDictionary.AddDictionaryToApp("./Languages/English.xaml");
                RefreshTourList(tourHandler.GetTours());
            });

            SelectGermanCommand = new RelayCommand(_ =>
            {
                _tourDictionary.AddDictionaryToApp("./Languages/Deutsch.xaml");
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
                    var currentTourViewModel = SelectedViewModel as CurrentTourViewModel;
                    if(currentTourViewModel.CurrentTour.Id == item.Id)
                    {
                        currentTourViewModel.CurrentTour = item;
                    }
                    currentTourViewModel.RefreshTourLogList(_tourHandler.GetTourLogs(CurrentTour), _tourHandler, _tourDictionary);
                }

                ToursList.Add(item);
            }
        }
    }
}
