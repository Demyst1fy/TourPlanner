﻿using System.Windows;
using TourPlanner.BusinessLayer;
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
            this.DataContext = new TourVM(TourHandlerSingleton.GetHandler());
        }
    }
}
