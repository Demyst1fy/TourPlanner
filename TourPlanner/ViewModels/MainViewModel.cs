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
        private Tour currentItem;
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

        public Tour CurrentItem
        {
            get { return currentItem; }
            set
            {
                if ((currentItem != value) && (value != null))
                {
                    currentItem = value;
                    this.SelectedViewModel = new CurrentTourViewModel();
                    RaisePropertyChangedEvent(nameof(CurrentItem));
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
            foreach (Tour item in tourHandler.GetItems())
            {
                Items.Add(item);
            }
        }
    }
}
