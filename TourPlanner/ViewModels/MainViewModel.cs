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
            tourHandler = TourHandler.GetHandler();
            SelectedViewModel = new WelcomeViewModel();
            Items = new ObservableCollection<Tour>();
            foreach (Tour item in tourHandler.GetTours())
            {
                Items.Add(item);
            }

            SearchCommand = new RelayCommand(o =>
            {
                if (string.IsNullOrEmpty(SearchName))
                    return;

                IEnumerable<Tour> items = tourHandler.SearchForTour(SearchName);
                Items.Clear();

                foreach (Tour item in items)
                {
                    Items.Add(item);
                }
            });

            ClearCommand = new RelayCommand(o =>
            {
                Items.Clear();
                SearchName = "";
                SelectedViewModel = new WelcomeViewModel();

                foreach (Tour item in tourHandler.GetTours())
                {
                    Items.Add(item);
                }
            });

            AddTourCommand = new RelayCommand(o =>
            {
                SelectedViewModel = new AddTourViewModel(this);
            });
        }
    }
}
