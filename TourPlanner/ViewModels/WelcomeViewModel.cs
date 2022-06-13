using System.Windows.Input;
using TourPlanner.Utils;

namespace TourPlanner.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        public ICommand AddTourCommand { get; private set; }

        public WelcomeViewModel(MainViewModel mainViewModel)
        {
            AddTourCommand = new RelayCommand(o =>
            {
                mainViewModel.SelectedViewModel = new AddTourViewModel(mainViewModel);
            });
        }
    }
}
