using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.Utils;

namespace TourPlanner.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ITourHandler tourHandler;
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
                    this.SelectedViewModel = new CurrentTourViewModel(this);
                }
            }
        }

        public MainViewModel()
        {
            ResourceDictionary dictionary = new ResourceDictionary();
            dictionary.Source = new Uri("./Languages/English.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(dictionary);

            tourHandler = TourHandler.GetHandler();
            SelectedViewModel = new WelcomeViewModel(this);
            ToursList = new ObservableCollection<Tour>();

            foreach (Tour item in tourHandler.GetTours())
            {
                item.TransportType = ChangeTransportTypeToSelectedLanguage(item.TransportType);

                ToursList.Add(item);
            }

            SearchCommand = new RelayCommand(o =>
            {
                if (string.IsNullOrEmpty(SearchName))
                    return;

                IEnumerable<Tour> items = tourHandler.SearchForTour(SearchName);
                RefreshTourList(items);
                SelectedViewModel = new WelcomeViewModel(this);
            });

            ClearCommand = new RelayCommand(o =>
            {
                SearchName = "";
                RefreshTourList(tourHandler.GetTours());
                SelectedViewModel = new WelcomeViewModel(this);
            });

            AddTourCommand = new RelayCommand(o =>
            {
                SelectedViewModel = new AddTourViewModel(this);
            });

            SelectEnglishCommand = new RelayCommand(o =>
            {
                ResourceDictionary dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("./Languages/English.xaml", UriKind.Relative);
                Application.Current.Resources.MergedDictionaries.Add(dictionary);
                RefreshTourList(tourHandler.GetTours());
            });

            SelectGermanCommand = new RelayCommand(o =>
            {
                ResourceDictionary dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("./Languages/Deutsch.xaml", UriKind.Relative);
                Application.Current.Resources.MergedDictionaries.Add(dictionary);
                RefreshTourList(tourHandler.GetTours());
            });
        }

        public void RefreshTourList(IEnumerable<Tour> items)
        {
            ToursList.Clear();
            foreach (Tour item in items)
            {

                item.TransportType = ChangeTransportTypeToSelectedLanguage(item.TransportType);

                if (SelectedViewModel is CurrentTourViewModel)
                {
                    var currentTourViewModel = SelectedViewModel as CurrentTourViewModel;
                    currentTourViewModel.RefreshTourLogList(tourHandler.GetTourLogs(CurrentTour));
                }

                ToursList.Add(item);
            }
        }

        private string ChangeTransportTypeToSelectedLanguage(string transportType)
        {
            if (transportType == "Car")
                return (string)Application.Current.Resources["StringTourCar"];
            else if (transportType == "Foot")
                return (string)Application.Current.Resources["StringTourFoot"];
            else if (transportType == "Bicycle")
                return (string)Application.Current.Resources["StringTourBicycle"];
            else
                return (string)Application.Current.Resources["StringTourCar"];
        }
    }
}
