using System;
using System.Windows.Input;
using TourPlanner.BusinessLayer.TourHandler;
using TourPlanner.Models;
using TourPlanner.BusinessLayer.DictionaryHandler;
using TourPlanner.Utils;

namespace TourPlanner.ViewModels
{
    public class AddTourLogViewModel : BaseViewModel
    {
        private Tour currentTour;
        public Tour CurrentTour
        {
            get
            {
                return currentTour;
            }
            set
            {
                currentTour = value;
                RaisePropertyChangedEvent(nameof(CurrentTour));
            }
        }

        private string comment = string.Empty;
        public string Comment
        {
            get { return comment; }
            set
            {
                if ((comment != value))
                {
                    comment = value;
                    RaisePropertyChangedEvent(nameof(Comment));
                }
            }
        }

        private string difficulty = string.Empty;
        public string Difficulty
        {
            get { return difficulty; }
            set
            {
                if ((difficulty != value))
                {
                    difficulty = value;
                    RaisePropertyChangedEvent(nameof(Difficulty));
                }
            }
        }
        private TimeSpan totalTime;
        public TimeSpan TotalTime
        {
            get { return totalTime; }
            set
            {
                if ((totalTime != value))
                {
                    totalTime = value;
                    RaisePropertyChangedEvent(nameof(TotalTime));
                }
            }
        }

        private int rating = 0;
        public int Rating
        {
            get { return rating; }
            set
            {
                if ((rating != value))
                {
                    rating = value;
                    RaisePropertyChangedEvent(nameof(Rating));
                }
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public AddTourLogViewModel(MainViewModel mainViewModel, ITourHandler tourHandler, ITourDictionary tourDictionary)
        {
            CurrentTour = mainViewModel.CurrentTour;
            Difficulty = tourDictionary.GetResourceFromDictionary("StringTourLogsDifficultyEasy");
            Rating = 5;

            AddCommand = new RelayCommand(_ => {
                Difficulty = tourDictionary.ChangeDifficultyToPassBL(Difficulty);
                TourLog newTourLog = new TourLog(Comment, Difficulty, TotalTime, Rating);
                tourHandler.AddNewTourLog(CurrentTour.Id, newTourLog);
                mainViewModel.SelectedViewModel = new CurrentTourViewModel(mainViewModel, tourHandler, tourDictionary);
            });

            CancelCommand = new RelayCommand(_ => {
                mainViewModel.SelectedViewModel = new CurrentTourViewModel(mainViewModel, tourHandler, tourDictionary);
            });
        }
    }
}
