using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Refit;
using ScuffedCountdown.Client.APIs;
using ScuffedCountdown.Client.Extensions;
using ScuffedCountdown.Client.Services;

namespace ScuffedCountdown.Client.Components.Molecules
{
    public partial class LettersRoundEvaluator : ComponentBase
    {
        [Inject]
        private IFreeDictionaryApi _DictionaryApi { get; set; } = default!;
        [Inject]
        private IUrbanDictionaryApi _UrbanDictionaryApi { get; set; } = default!;
        [Inject]
        private IJSRuntime _Js { get; set; } = default!;
        [Inject]
        private CommonJsService _CommonJs { get; set; } = default!;
        [Inject]
        private StateManager _StateManager { get; set; } = default!;

        [Parameter, EditorRequired]
        public List<char> Letters { get; set; } = default!;
        private List<char> _AvailableLetters = new();

        private List<DefinitionLookupModel> _DefinitionsLookups { get; set; } = new();
        private IJSObjectReference _JsModule = default!;

        private string _DictionaryInputId = Guid.NewGuid().Short();
        private string? _DictionaryInputValue { get; set; }
        private string? _LastInput;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            foreach (var letter in Letters)
                _AvailableLetters.Add(letter);
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _JsModule = await _Js.ImportJsModule();
        }

        private async void DictionaryInput_OnKeyUp(KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case "Enter":
                    if (_DictionaryInputValue != null && _DictionaryInputValue.Count() > 0)
                    {
                        _DefinitionsLookups.Clear();
                        var word = _DictionaryInputValue;
                        var state = await _StateManager.GetState();

                        if (state.UserSettings.UseFreeDictionary)
                        {
                            var freeDictionaryDefinition = await GetFreeDictionaryDefinition(word);
                            if (freeDictionaryDefinition != null)
                                _DefinitionsLookups.Add(freeDictionaryDefinition);
                        }

                        if (state.UserSettings.UseUrbanDictionary)
                        {
                            var urbanDictionaryDefinition = await GetUrbanDictionaryDefinition(word);
                            if (urbanDictionaryDefinition != null)
                                _DefinitionsLookups.Add(urbanDictionaryDefinition);
                        }

                        if (!_DefinitionsLookups.Any())
                            await _CommonJs.PlayErrorSound();
                    }
                    StateHasChanged();
                    break;
            }
        }

        private async Task SetDictionaryInputValue(string value)
        {
            await _JsModule.InvokeVoidAsync("setDictionaryInputValue", value, _DictionaryInputId);
        }

        private async void DictionaryInput_OnInput(ChangeEventArgs e)
        {
            var value = (string)(e.Value ?? "");

            if (_LastInput == null) // First input
                _LastInput = "";

            if (Math.Abs(_LastInput.Count() - value.Count()) > 1)
            {
                // many paste or big delete we no like
                await SetDictionaryInputValue(_LastInput);
                return;
            }

            if (_LastInput.Count() > value.Count()) // Backspace
            {
                var deletedChar = _LastInput.Last();
                _LastInput = value;
                _AvailableLetters.Add(deletedChar);
                return;
            }

            var input = char.ToUpper(value.Last());

            string newInput;
            if (_AvailableLetters.Contains(input))
            {
                newInput = _LastInput + input;
                _AvailableLetters.Remove(input);
            }
            else
            {
                newInput = _LastInput;
                await _CommonJs.PlayErrorSound();
            }

            await SetDictionaryInputValue(newInput);
            _LastInput = newInput;
        }

        private async Task<DefinitionLookupModel?> GetUrbanDictionaryDefinition(string word)
        {
            try
            {

                var result = await _UrbanDictionaryApi.GetDefinition(word);
                result.List.RemoveAll(x => x.Word.ToUpper() != word.ToUpper());
                result.List.OrderByDescending(x => x.Score);

                return result.List.Any()
                    ? GetLookupModel(result, word)
                    : null;
            }
            catch
            {
                return null;
            }
        }

        private async Task<DefinitionLookupModel?> GetFreeDictionaryDefinition(string word)
        {
            try
            {
                var result = await _DictionaryApi.GetDefinition(FreeDictionaryApiLanguages.English, word);
                return GetLookupModel(result, word);
            }
            catch
            {
                return null;
            }
        }

        private DefinitionLookupModel GetLookupModel(List<FreeDictionaryResponse> response, string word)
        {
            var definition = response.First();

            var model = new DefinitionLookupModel
            {
                Dictionary = "FreeDictionary",
                Word = word,
                Phonetic = definition.Phonetics.First().Text,
            };

            foreach (var meaning in definition.Meanings)
            {
                var definitionModel = new DefinitionModel
                {
                    PartOfSpeech = meaning.PartOfSpeech,
                    Definitions = meaning.Definitions.Select(x => x.Definition).ToList()
                };
                model.Definitions.Add(definitionModel);
            }
            return model;
        }

        private DefinitionLookupModel GetLookupModel(UrbanDictionaryResponse response, string word)
        {
            var model = new DefinitionLookupModel
            {
                Dictionary = "UrbanDictionary",
                Word = word,
            };

            foreach (var definition in response.List)
            {
                model.Definitions.Add(new DefinitionModel
                {
                    Definitions = new()
                    {
                        definition.Definition
                    }
                });
            }

            return model;
        }
    }

    public class DefinitionLookupModel
    {
        public string Dictionary { get; set; } = default!;
        public string Word { get; set; } = default!;
        public string? Phonetic { get; set; }

        public List<DefinitionModel> Definitions { get; set; } = new();
    }

    public class DefinitionModel
    {
        public string? PartOfSpeech { get; set; }
        public List<string> Definitions { get; set; } = default!;
    }
}
