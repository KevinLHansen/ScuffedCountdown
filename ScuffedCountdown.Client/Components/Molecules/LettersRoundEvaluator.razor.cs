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
        private IJSRuntime _Js { get; set; } = default!;
        [Inject]
        private CommonJsService _CommonJs { get; set; } = default!;

        [Parameter, EditorRequired]
        public List<char> Letters { get; set; } = default!;
        private List<char> _AvailableLetters = new();

        private FreeDictionaryResponse? _Definition { get; set; }
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
                        try
                        {
                            var dictionaryResult = await _DictionaryApi.GetDefinition(FreeDictionaryApiLanguages.English, _DictionaryInputValue);
                            _Definition = dictionaryResult.FirstOrDefault();
                        }
                        catch (ApiException)
                        {
                            // No definition for word
                            _Definition = null;
                            await _CommonJs.PlayErrorSound();
                        }
                    }
                    break;
            }
            StateHasChanged();
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
    }
}
