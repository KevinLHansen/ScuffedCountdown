using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Refit;
using ScuffedCountdown.Client.APIs;

namespace ScuffedCountdown.Client.Components.Molecules
{
    public partial class LettersRoundEvaluator : ComponentBase
    {
        [Inject]
        private IFreeDictionaryApi _DictionaryApi { get; set; } = default!;

        [Parameter, EditorRequired]
        public List<char> Letters { get; set; } = default!;

        private FreeDictionaryResponse? _Definition { get; set; }

        private string? _DictionaryInputValue { get; set; }

        private void DictionaryInput_OnInput(ChangeEventArgs e)
        {
            if (e.Value == null)
            {
                _DictionaryInputValue = null;
                return;
            }
            _DictionaryInputValue = (string)e.Value;
        }

        private async void DictionaryInput_OnKeyDown(KeyboardEventArgs e)
        {
            if (e.Key != "Enter")
                return;

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
                }
            }
            StateHasChanged();
        }
    }
}
