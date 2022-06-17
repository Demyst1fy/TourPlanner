using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.ViewModels;
using TourPlanner.DictionaryHandler;

namespace TourPlanner.Unittest
{
    public class ViewModelTest
    {
        private MainViewModel viewModel;
        private Mock<ITourHandler> mockTourHandler;
        private Mock<ITourDictionary> mockTourDictionary;

        private Tour testTour1;
        private Tour testTour2;
        private TourLog testTourLog1;

        [SetUp]
        public void Setup()
        {
            mockTourHandler = new Mock<ITourHandler>();
            mockTourDictionary = new Mock<ITourDictionary>();
            viewModel = new MainViewModel(mockTourHandler.Object, mockTourDictionary.Object);
            testTour1 = new Tour(1, "TestTour1", "Description1", "Wien", "Graz", "Car", 200, new TimeSpan(2, 0, 0));
            testTour2 = new Tour(1, "TestTour2", "Description2", "Graz", "Wien", "Car", 200, new TimeSpan(2, 0, 0));
            testTourLog1 = new TourLog("Comment1", "Medium", new TimeSpan(2, 30, 30), 4);
        }

        [Test]
        public void Test_SelectedViewModelAtStart()
        {
            Assert.AreEqual(viewModel.SelectedViewModel.GetType(), typeof(WelcomeViewModel));
        }

        [Test]
        public void Test_TourListAtStart()
        {
            List<Tour> tourList = new List<Tour>();
            tourList.Add(testTour1);
            tourList.Add(testTour2);
            mockTourHandler.Setup(mock => mock.GetTours()).Returns(tourList);

            viewModel = new MainViewModel(mockTourHandler.Object, mockTourDictionary.Object);

            Assert.AreEqual(2, viewModel.ToursList.Count);
        }

        [Test]
        public void Test_SwitchLanguage()
        {
            viewModel.SelectGermanCommand.Execute(viewModel);
            mockTourDictionary.Verify(mock => mock.AddDictionaryToApp("./Languages/Deutsch.xaml"), Times.Once());

            viewModel.SelectEnglishCommand.Execute(viewModel);
            mockTourDictionary.Verify(mock => mock.AddDictionaryToApp("./Languages/English.xaml"), Times.Once());

            viewModel.SelectGermanCommand.Execute(viewModel);
            mockTourDictionary.Verify(mock => mock.AddDictionaryToApp("./Languages/Deutsch.xaml"), Times.Exactly(2));
        }

        [Test]
        public void Test_SearchForTourWhenEntryIsEmpty()
        {
            viewModel.SearchName = string.Empty;

            viewModel.SearchCommand.Execute(viewModel);

            mockTourHandler.Verify(mock => mock.SearchForTour(viewModel.SearchName), Times.Never());
        }

        [Test]
        public void Test_SearchForTour()
        {
            List<Tour> tourList = new List<Tour>();
            tourList.Add(testTour1);
            mockTourHandler.Setup(mock => mock.SearchForTour(testTour1.Name)).Returns(tourList);

            viewModel.SearchName = "TestTour1";
            viewModel.SearchCommand.Execute(viewModel);

            Assert.AreEqual(1, viewModel.ToursList.Count);
        }

        [Test]
        public void Test_SearchForTourButListIsEmpty()
        {
            List<Tour> tourList = new List<Tour>();
            tourList.Add(testTour1);
            mockTourHandler.Setup(mock => mock.SearchForTour(testTour1.Name)).Returns(tourList);

            viewModel.SearchName = "asdf";
            viewModel.SearchCommand.Execute(viewModel);

            Assert.AreEqual(0, viewModel.ToursList.Count);
        }

        [Test]
        public void Test_SelectedViewModelWhenTourSelected()
        {
            List<Tour> tourList = new List<Tour>();
            List<TourLog> tourLogListFrom1 = new List<TourLog>();
            mockTourHandler.Setup(mock => mock.AddNewTour(testTour1));
            mockTourHandler.Setup(mock => mock.AddNewTourLog(testTour1.Id, testTourLog1));
            tourLogListFrom1.Add(testTourLog1);
            mockTourHandler.Setup(mock => mock.GetTourLogs(testTour1)).Returns(tourLogListFrom1);
            mockTourHandler.Setup(mock => mock.GetAllTourLogs()).Returns(tourLogListFrom1);

            viewModel.CurrentTour = testTour1;

            Assert.AreEqual(typeof(CurrentTourViewModel), viewModel.SelectedViewModel.GetType());
        }

        [Test]
        public void Test_SelectTourOnList()
        {
            List<Tour> tourList = new List<Tour>();
            List<TourLog> tourLogListFrom1 = new List<TourLog>();
            mockTourHandler.Setup(mock => mock.AddNewTour(testTour1));
            mockTourHandler.Setup(mock => mock.AddNewTourLog(testTour1.Id, testTourLog1));
            tourLogListFrom1.Add(testTourLog1);
            mockTourHandler.Setup(mock => mock.GetTourLogs(testTour1)).Returns(tourLogListFrom1);
            mockTourHandler.Setup(mock => mock.GetAllTourLogs()).Returns(tourLogListFrom1);

            viewModel.CurrentTour = testTour1;
            var currentVm = viewModel.SelectedViewModel as CurrentTourViewModel;

            Assert.AreEqual("Car", currentVm.CurrentTour.TransportType);
        }

        [Test]
        public void Test_SelectTourOnListThenChangeLanguage()
        {
            List<Tour> tourList = new List<Tour>();
            List<TourLog> tourLogListFrom1 = new List<TourLog>();
            mockTourHandler.Setup(mock => mock.AddNewTour(testTour1));
            mockTourHandler.Setup(mock => mock.AddNewTourLog(testTour1.Id, testTourLog1));
            tourLogListFrom1.Add(testTourLog1);
            mockTourHandler.Setup(mock => mock.GetTourLogs(testTour1)).Returns(tourLogListFrom1);
            mockTourHandler.Setup(mock => mock.GetAllTourLogs()).Returns(tourLogListFrom1);

            viewModel.CurrentTour = testTour1;
            var currentVm = viewModel.SelectedViewModel as CurrentTourViewModel;

            Assert.AreEqual("Car", currentVm.CurrentTour.TransportType);

            viewModel.SelectGermanCommand.Execute(viewModel);
            mockTourDictionary.Setup(mock => mock.AddDictionaryToApp("./Languages/Deutsch.xaml"));
            mockTourDictionary.Setup(mock => mock.ChangeTransportTypeToSelectedLanguage(testTour1.TransportType)).Returns("Auto");

            currentVm.CurrentTour.TransportType = mockTourDictionary.Object.ChangeTransportTypeToSelectedLanguage(testTour1.TransportType);

            Assert.AreEqual("Auto", currentVm.CurrentTour.TransportType);
        }

        [Test]
        public void Test_SelectTourOnListTwoTimes()
        {
            List<Tour> tourList = new List<Tour>();
            List<TourLog> tourLogListFrom1 = new List<TourLog>();
            mockTourHandler.Setup(mock => mock.AddNewTour(testTour1));
            mockTourHandler.Setup(mock => mock.AddNewTourLog(testTour1.Id, testTourLog1));
            tourLogListFrom1.Add(testTourLog1);
            mockTourHandler.Setup(mock => mock.GetTourLogs(testTour1)).Returns(tourLogListFrom1);
            mockTourHandler.Setup(mock => mock.GetAllTourLogs()).Returns(tourLogListFrom1);

            viewModel.CurrentTour = testTour1;
            var currentVm = viewModel.SelectedViewModel as CurrentTourViewModel;
            Assert.AreEqual("TestTour1", currentVm.CurrentTour.Name);

            viewModel.CurrentTour = testTour2;
            currentVm = viewModel.SelectedViewModel as CurrentTourViewModel;
            Assert.AreEqual("TestTour2", currentVm.CurrentTour.Name);
        }

        [Test]
        public void Test_CalculateTourAttributes()
        {
            Console.WriteLine(testTourLog1);
            List<Tour> tourList = new List<Tour>();
            List<TourLog> tourLogListFrom1 = new List<TourLog>();
            mockTourHandler.Setup(mock => mock.AddNewTour(testTour1));
            mockTourHandler.Setup(mock => mock.AddNewTourLog(testTour1.Id, testTourLog1));

            tourLogListFrom1.Add(testTourLog1);
            mockTourHandler.Setup(mock => mock.GetTourLogs(testTour1)).Returns(tourLogListFrom1);
            mockTourHandler.Setup(mock => mock.GetAllTourLogs()).Returns(tourLogListFrom1);
            mockTourDictionary.Setup(mock => mock.ChangeDifficultyToSelectedLanguage(testTourLog1.Difficulty)).Returns(testTourLog1.Difficulty);
            mockTourDictionary.Setup(mock => mock.GetResourceFromDictionary("StringTourLogsDifficultyMedium")).Returns("Medium");

            viewModel.CurrentTour = testTour1;
            var currentVm = viewModel.SelectedViewModel as CurrentTourViewModel;
            currentVm.CurrentTourLog = testTourLog1;

            Assert.AreEqual("Medium", currentVm.CurrentTourLog.Difficulty);

            // currentTourLogs.Count() / GetAllTourLogs().Count
            // 1 / 1 = 1 * 100 = 100%
            Assert.AreEqual(100, currentVm.Popularity);

            // (currentTourLogs.Count() + durationIndexFromTour + distanceIndexFromTour) / (difficultySumFromCurrentTourLogs + 5 + 5)
            // = (1 + 2 + 1) / (2 + 5 + 5) = 0.33 * 100 = 33%
            Assert.AreEqual(33, currentVm.ChildFriendliness);
        }


    }
}
