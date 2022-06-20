using System.Windows.Input;
using TourPlanner.BusinessLayer.TourHandler;
using TourPlanner.Utils;
using TourPlanner.BusinessLayer.DictionaryHandler;

namespace TourPlanner.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        public ICommand AddTourCommand { get; private set; }
        public ICommand GenerateTourSummarizeReportCommand { get; private set; }
        public ICommand ImportTourCommand { get; private set; }

        public WelcomeViewModel(MainViewModel mainViewModel)
        {
            AddTourCommand = new RelayCommand(_ =>
            {
                mainViewModel.AddTourCommand.Execute(_);
            });

            GenerateTourSummarizeReportCommand = new RelayCommand(_ =>
            {
                mainViewModel.GenerateTourSummarizeReportCommand.Execute(_);
            });

            ImportTourCommand = new RelayCommand(_ =>
            {
                mainViewModel.ImportTourCommand.Execute(_);
            });
        }
    }
}
