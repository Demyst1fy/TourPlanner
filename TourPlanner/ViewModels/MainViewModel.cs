using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ITourHandler tourHandler;
        private Tour currentTour;

        private string searchName;
        public ObservableCollection<Tour> Items { get; set; }

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
                    this.SelectedViewModel = new CurrentTourViewModel(currentTour);
                    RaisePropertyChangedEvent(nameof(CurrentTour));
                }
            }
        }

        public ICommand SearchCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand AddTourCommand { get; set; }

        private BaseViewModel? _selectedViewModel;

        public BaseViewModel SelectedViewModel
        {
            get
            {
                return _selectedViewModel;
            }
            set
            {
                _selectedViewModel = value;
                RaisePropertyChangedEvent(nameof(SelectedViewModel));
            }
        }

        public MainViewModel(ITourHandler tourHandler)
        {
            this.tourHandler = tourHandler;

            Items = new ObservableCollection<Tour>();

            this.SearchCommand = new RelayCommand(o => {
                IEnumerable<Tour> items = this.tourHandler.SearchForTour(SearchName);
                Items.Clear();
                foreach (Tour item in items)
                {
                    Items.Add(item);
                }
            });

            this.ClearCommand = new RelayCommand(o => {

                Items.Clear();
                SearchName = "";

                FillListView();
            });

            this.AddTourCommand = new RelayCommand(o => {

                this.SelectedViewModel = new AddTourViewModel();
            });

            InitListView();
        }


        public void InitListView()
        {
            Items = new ObservableCollection<Tour>();
            FillListView();
        }

        private void FillListView()
        {
            foreach (Tour item in tourHandler.GetTours())
            {
                Items.Add(item);
            }
        }
    }
}
