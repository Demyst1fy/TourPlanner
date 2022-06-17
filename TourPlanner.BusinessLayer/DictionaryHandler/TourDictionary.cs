using System;
using System.Windows;

namespace TourPlanner.DictionaryHandler
{
    public class TourDictionary : ITourDictionary
    {
        public TourDictionary()
        {
            AddDictionaryToApp("./Languages/English.xaml");
        }

        public void AddDictionaryToApp(string path)
        {
            ResourceDictionary dictionary = new ResourceDictionary();
            dictionary.Source = new Uri(path, UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }

        public string GetResourceFromDictionary(string index)
        {
            return (string)Application.Current.Resources[index];
        }

        public string ChangeTransportTypeToSelectedLanguage(string transportType)
        {
            if (transportType == "Car")
                return (string)Application.Current.Resources["StringTourCar"];
            else if (transportType == "Foot")
                return (string)Application.Current.Resources["StringTourFoot"];
            else if (transportType == "Bicycle")
                return (string)Application.Current.Resources["StringTourBicycle"];
            else
                return (string)Application.Current.Resources["StringTourCar"];
        }

        public string ChangeTransportTypeToPassBL(string transportType)
        {
            if (transportType == (string)Application.Current.Resources["StringTourCar"])
                return "Car";
            else if (transportType == (string)Application.Current.Resources["StringTourFoot"])
                return "Foot";
            else if (transportType == (string)Application.Current.Resources["StringTourBicycle"])
                return "Bicycle";
            else
                return "Car";
        }

        public string ChangeDifficultyToSelectedLanguage(string difficulty)
        {
            if (difficulty == "Easy")
                return (string)Application.Current.Resources["StringTourLogsDifficultyEasy"];
            else if (difficulty == "Medium")
                return (string)Application.Current.Resources["StringTourLogsDifficultyMedium"];
            else if (difficulty == "Hard")
                return (string)Application.Current.Resources["StringTourLogsDifficultyHard"];
            else
                return (string)Application.Current.Resources["StringTourLogsDifficultyEasy"];
        }

        public string ChangeDifficultyToPassBL(string difficulty)
        {
            if (difficulty == (string)Application.Current.Resources["StringTourLogsDifficultyEasy"])
                return "Easy";
            else if (difficulty == (string)Application.Current.Resources["StringTourLogsDifficultyMedium"])
                return "Medium";
            else if (difficulty == (string)Application.Current.Resources["StringTourLogsDifficultyHard"])
                return "Hard";
            else
                return "Easy";
        }
    }
}
