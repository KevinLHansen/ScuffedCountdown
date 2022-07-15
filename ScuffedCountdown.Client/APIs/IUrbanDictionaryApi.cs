using Refit;

namespace ScuffedCountdown.Client.APIs
{
    public interface IUrbanDictionaryApi
    {
        public static Uri BaseUri = new Uri("https://api.urbandictionary.com/v0");

        [Get("/define?term={word}")]
        Task<UrbanDictionaryResponse> GetDefinition(string word);
    }

    public class UrbanDictionaryResponse
    {
        public List<UrbanDictionaryDefinition> List { get; set; } = new();
    }

    public class UrbanDictionaryDefinition
    {
        public string Word { get; set; } = default!;
        public string Author { get; set; } = default!;
        public string Definition { get; set; } = default!;
        public int Thumbs_Up { get; set; }
        public int Thumbs_Down { get; set; }
        public int Score => Thumbs_Up - Thumbs_Down;
    }
}
