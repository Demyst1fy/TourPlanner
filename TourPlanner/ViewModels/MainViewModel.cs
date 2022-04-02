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
        private BaseViewModel selectedViewModel;
        private string searchName;
        private Tour currentTour;
        public ObservableCollection<Tour> Items { get; private set; }
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
            this.tourHandler = TourHandler.GetHandler();
            this.SelectedViewModel = new WelcomeViewModel();
            this.Items = new ObservableCollection<Tour>();
            FillListView(tourHandler.GetTours());

            this.SearchCommand = new RelayCommand(o =>
            {
                IEnumerable<Tour> items = this.tourHandler.SearchForTour(SearchName);
                Items.Clear();

                FillListView(items);
            });

            this.ClearCommand = new RelayCommand(o =>
            {
                Items.Clear();
                SearchName = "";
                this.SelectedViewModel = new WelcomeViewModel();

                FillListView(tourHandler.GetTours());
            });

            this.AddTourCommand = new RelayCommand(o =>
            {
                this.SelectedViewModel = new AddTourViewModel(this);
            });
        }

        private void FillListView(IEnumerable<Tour> items)
        {
            foreach (Tour item in items)
            {
                Items.Add(item);
            }
        }
    }
}
