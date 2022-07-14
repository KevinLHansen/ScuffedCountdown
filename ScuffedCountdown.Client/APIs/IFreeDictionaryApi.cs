using Refit;

namespace ScuffedCountdown.Client.APIs
{
    public interface IFreeDictionaryApi
    {
        public static Uri BaseUri = new Uri("https://api.dictionaryapi.dev/api/v2/entries");

        [Get("/{language}/{word}")]
        Task<List<FreeDictionaryResponse>> GetDefinition(string language, string word);
    }

    public class FreeDictionaryResponse
    {
        public string Word { get; set; } = default!;
        public List<PhoneticModel> Phonetics { get; set; } = new();
        public List<MeaningModel> Meanings { get; set; } = new();
    }

    public class PhoneticModel
    {
        public string? Text { get; set; }
    }

    public class MeaningModel
    {
        public string PartOfSpeech { get; set; } = default!;
        public List<DefinitionModel> Definitions { get; set; } = new();
    }

    public class DefinitionModel
    {
        public string Definition { get; set; } = default!;
        public string Example { get; set; } = default!;
        public List<string> Synonyms { get; set; } = new();
    }

    /// <summary>
    /// Languages supported by Free Dictionary API
    /// </summary>
    public static class FreeDictionaryApiLanguages
    {
        public static string English = "en";
    }
}
