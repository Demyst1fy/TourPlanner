using System.Windows.Input;
using TourPlanner.BusinessLayer.TourHandler;
using TourPlanner.Utils;
using TourPlanner.BusinessLayer.DictionaryHandler;

namespace TourPlanner.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        public ICommand AddTourCommand { get; private set; }

        public WelcomeViewModel(MainViewModel mainViewModel, ITourHandler tourHandler, ITourDictionary tourDictionary)
        {
            AddTourCommand = new RelayCommand(_ =>
            {
                mainViewModel.SelectedViewModel = new AddTourViewModel(mainViewModel, tourHandler, tourDictionary);
            });
        }
    }
}
