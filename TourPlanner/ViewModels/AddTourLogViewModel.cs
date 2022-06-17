using System;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.DictionaryHandler;
using TourPlanner.Utils;

namespace TourPlanner.ViewModels
{
    public class AddTourLogViewModel : BaseViewModel
    {
        private Tour currentTour;

        private string comment;
        private string difficulty;
        private TimeSpan totalTime;
        private int rating;
        public ICommand AddCommand { get; set; }
        public ICommand CancelCommand { get; set; }

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

        public AddTourLogViewModel(MainViewModel mainViewModel, ITourHandler tourHandler, ITourDictionary tourDictionary)
        {
            Difficulty = tourDictionary.GetResourceFromDictionary("StringTourLogsDifficultyEasy");
            Rating = 5;

            tourHandler = TourHandler.GetHandler();
            CurrentTour = mainViewModel.CurrentTour;

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
