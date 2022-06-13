using System;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.BusinessLayer.JsonClasses;
using TourPlanner.Models;
using TourPlanner.Models.Enums;
using TourPlanner.Utils;

namespace TourPlanner.ViewModels
{
    public class AddTourLogViewModel : BaseViewModel
    {
        private ITourHandler tourHandler;
        private Visibility isError;
        private Tour currentTour;

        private string comment;
        private Difficulty difficulty;
        private TimeSpan totalTime;
        private int rating;
        public ICommand AddCommand { get; set; }

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

        public Difficulty Difficulty
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

        public Visibility IsError
        {
            get
            {
                return isError;
            }
            set
            {
                isError = value;
                RaisePropertyChangedEvent(nameof(IsError));
            }
        }

        public AddTourLogViewModel(MainViewModel mainViewModel)
        {
            Difficulty = Difficulty.Easy;
            Rating = 5;

            tourHandler = TourHandler.GetHandler();
            IsError = Visibility.Hidden;
            CurrentTour = mainViewModel.CurrentTour;

            AddCommand = new RelayCommand(o => {
                TourLog newTourLog = new TourLog(Comment, Difficulty, TotalTime, Rating);

                /*if (newTour == null)
                {
                    IsError = Visibility.Visible;
                    return;
                }*/

                tourHandler.AddNewTourLog(CurrentTour.Id, newTourLog);

                mainViewModel.SelectedViewModel = new WelcomeViewModel(mainViewModel);
            });
        }
    }
}
