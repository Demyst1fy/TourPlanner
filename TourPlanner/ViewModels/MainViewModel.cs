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
                    RaisePropertyChangedEvent(nameof(CurrentTour));
                    this.SelectedViewModel = new CurrentTourViewModel(CurrentTour, Items, this);
                }
            }
        }

        public ICommand SearchCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand AddTourCommand { get; set; }

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

        public MainViewModel(ITourHandler tourHandler)
        {
            this.tourHandler = tourHandler;

            Items = new ObservableCollection<Tour>();
            FillListView();

            this.SelectedViewModel = new WelcomeViewModel();


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

                this.SelectedViewModel = new WelcomeViewModel();

                FillListView();
            });

            this.AddTourCommand = new RelayCommand(o => {

                this.SelectedViewModel = new AddTourViewModel(Items, this);
            
            });
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
