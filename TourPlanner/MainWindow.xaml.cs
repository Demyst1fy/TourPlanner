using System.Configuration;
using System.Windows;
using TourPlanner.BusinessLayer.TourHandler;
using TourPlanner.BusinessLayer.DictionaryHandler;
using TourPlanner.BusinessLayer.Logger;
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
            var tourHandler = TourHandlerFactory.GetHandler();

            string language = ConfigurationManager.AppSettings["Language"] ?? "English";
            var dictionary = new TourDictionary(language);

            var logger = Log4NetLoggerFactory.GetLogger();

            DataContext = new MainViewModel(tourHandler, dictionary, logger);

            InitializeComponent();
        }
    }
}
