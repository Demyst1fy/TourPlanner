namespace TourPlanner.DictionaryHandler
{
    public interface ITourDictionary
    {
        void AddDictionaryToApp(string path);
        string GetResourceFromDictionary(string index);
        string ChangeTransportTypeToSelectedLanguage(string transportType);
        string ChangeTransportTypeToPassBL(string transportType);
        string ChangeDifficultyToSelectedLanguage(string difficulty);
        string ChangeDifficultyToPassBL(string difficulty);
    }
}
