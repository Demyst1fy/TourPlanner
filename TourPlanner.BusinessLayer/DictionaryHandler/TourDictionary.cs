using System;
using System.Configuration;
using System.Windows;

namespace TourPlanner.BusinessLayer.DictionaryHandler
{
    public class TourDictionary : ITourDictionary
    {
        public TourDictionary(string language)
        {
            AddDictionaryToApp(language);
        }

        public void AddDictionaryToApp(string language)
        {
            string path = $"./Languages/{language}.xaml";
            ResourceDictionary dictionary = new ResourceDictionary();
            dictionary.Source = new Uri(path, UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(dictionary);

            SetLanguageSettings(language);
        }
        private void SetLanguageSettings(string language)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var appSettings = configFile.AppSettings.Settings;

            if (appSettings["Language"] != null)
            {
                appSettings["Language"].Value = language;
            }

            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
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
