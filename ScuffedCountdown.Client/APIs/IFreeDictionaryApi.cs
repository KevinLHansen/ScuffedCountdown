using Refit;

namespace ScuffedCountdown.Client.APIs
{
    public interface IFreeDictionaryApi
    {
        public static Uri BaseUri = new Uri("https://api.dictionaryapi.dev/api/v2");

        [Get("/entries/{language}/{word}")]
        Task<List<FreeDictionaryResponse>> GetDefinition(string language, string word);
    }

    public class FreeDictionaryResponse
    {
        public string Word { get; set; } = default!;
        public List<FreeDictionaryPhonetic> Phonetics { get; set; } = new();
        public List<FreeDictionaryMeaning> Meanings { get; set; } = new();
    }

    public class FreeDictionaryPhonetic
    {
        public string? Text { get; set; }
    }

    public class FreeDictionaryMeaning
    {
        public string PartOfSpeech { get; set; } = default!;
        public List<FreeDictionaryDefinition> Definitions { get; set; } = new();
    }

    public class FreeDictionaryDefinition
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
