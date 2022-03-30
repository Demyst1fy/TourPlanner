using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    public class AddTourViewModel : BaseViewModel
    {
        private string start;

        public string Start
        {
            get { return start; }
            set
            {
                if ((start != value))
                {
                    start = value;
                    RaisePropertyChangedEvent(nameof(Start));
                }
            }
        }

        private string end;

        public string End
        {
            get { return end; }
            set
            {
                if ((end != value))
                {
                    end = value;
                    RaisePropertyChangedEvent(nameof(End));
                }
            }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set
            {
                if ((description != value))
                {
                    description = value;
                    RaisePropertyChangedEvent(nameof(Description));
                }
            }
        }

        public ICommand AddTour { get; set; }

        private ITourHandler tourHandler;

        public AddTourViewModel()
        {
            this.tourHandler = TourHandlerSingleton.GetHandler();

            this.AddTour = new RelayCommand(o => {
                Console.WriteLine("hiiiiiiiii");
                this.tourHandler.AddNewTour(new Tour(Start, End, Description));
            });
        }

    }
}
