using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            tourHandler = TourHandler.GetHandler();
            SelectedViewModel = new WelcomeViewModel(this);
            ToursList = new ObservableCollection<Tour>();
            foreach (Tour item in tourHandler.GetTours())
            {
                ToursList.Add(item);
            }

            SearchCommand = new RelayCommand(o =>
            {
                if (string.IsNullOrEmpty(SearchName))
                    return;

                IEnumerable<Tour> items = tourHandler.SearchForTour(SearchName);
                RefreshTourList(items);
            });

            ClearCommand = new RelayCommand(o =>
            {
                SearchName = "";
                SelectedViewModel = new WelcomeViewModel(this);

                RefreshTourList(tourHandler.GetTours());
            });

            AddTourCommand = new RelayCommand(o =>
            {
                SelectedViewModel = new AddTourViewModel(this);
            });
        }

        public void RefreshTourList(IEnumerable<Tour> items)
        {
            ToursList.Clear();
            foreach (Tour item in items)
            {
                ToursList.Add(item);
            }
        }
    }
}
