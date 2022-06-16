using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.Utils;

namespace TourPlanner.ViewModels
{
    public class ModifyTourLogViewModel : BaseViewModel
    {
        private ITourHandler tourHandler;
        private string currentTourName;
        private TourLog currentTourLog;
        public ICommand ModifyCommand { get; set; }

        public string CurrentTourName
        {
            get
            {
                return currentTourName;
            }
            set
            {
                currentTourName = value;
                RaisePropertyChangedEvent(nameof(CurrentTourName));
            }
        }

        public TourLog CurrentTourLog
        {
            get
            {
                return currentTourLog;
            }
            set
            {
                currentTourLog = value;
                RaisePropertyChangedEvent(nameof(CurrentTourLog));
            }
        }

        public ModifyTourLogViewModel(MainViewModel mainViewModel, CurrentTourViewModel currentTourViewModel)
        {
            tourHandler = TourHandler.GetHandler();
            CurrentTourLog = currentTourViewModel.CurrentTourLog;

            ModifyCommand = new RelayCommand(o => {

                ChangeDifficultyToPassBL();

                TourLog tourLog = new TourLog(CurrentTourLog.Id, CurrentTourLog.Datetime, CurrentTourLog.Comment, CurrentTourLog.Difficulty, CurrentTourLog.TotalTime, CurrentTourLog.Rating);

                tourHandler.ModifyTourLog(tourLog);

                mainViewModel.SelectedViewModel = new CurrentTourViewModel(mainViewModel);
                currentTourViewModel.RefreshTourLogList(tourHandler.GetTourLogs(mainViewModel.CurrentTour));
            });
        }

        public void ChangeDifficultyToPassBL()
        {
            if (CurrentTourLog.Difficulty == (string)Application.Current.Resources["StringTourLogsDifficultyEasy"])
                CurrentTourLog.Difficulty = "Easy";
            else if (CurrentTourLog.Difficulty == (string)Application.Current.Resources["StringTourLogsDifficultyMedium"])
                CurrentTourLog.Difficulty = "Medium";
            else if (CurrentTourLog.Difficulty == (string)Application.Current.Resources["StringTourLogsDifficultyHard"])
                CurrentTourLog.Difficulty = "Hard";
            else
                CurrentTourLog.Difficulty = "Easy";
        }
    }
}
