using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private Visibility isError;
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

        public ModifyTourLogViewModel(MainViewModel mainViewModel, CurrentTourViewModel currentTourViewModel)
        {
            tourHandler = TourHandler.GetHandler();
            IsError = Visibility.Hidden;
            CurrentTourLog = currentTourViewModel.CurrentTourLog;

            ModifyCommand = new RelayCommand(o => {
                TourLog tourLog = new TourLog(CurrentTourLog.Id, CurrentTourLog.Comment, CurrentTourLog.Difficulty, CurrentTourLog.TotalTime, CurrentTourLog.Rating);

                /*if (newTour == null)
                {
                    IsError = Visibility.Visible;
                    return;
                }*/

                tourHandler.ModifyTourLog(tourLog);

                mainViewModel.SelectedViewModel = new CurrentTourViewModel(mainViewModel);
                currentTourViewModel.RefreshTourLogList(tourHandler.GetTourLogs(mainViewModel.CurrentTour));
            });
        }
    }
}
