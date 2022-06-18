using System.Configuration;
using System.Windows;
using TourPlanner.BusinessLayer.TourHandler;
using TourPlanner.BusinessLayer.DictionaryHandler;
using TourPlanner.ViewModels;

namespace TourPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string language = ConfigurationManager.AppSettings["Language"] ?? "English";
            DataContext = new MainViewModel(TourHandlerFactory.GetHandler(), new TourDictionary(language));
        }
    }
}
