using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using ScuffedCountdown.Client.Extensions;
using ScuffedCountdown.Client.Services;
using System.Data;

namespace ScuffedCountdown.Client.Components.Molecules
{
    public partial class NumbersRoundEvaluator : ComponentBase
    {
        [Inject]
        private IJSRuntime _Js { get; set; } = default!;
        [Inject]
        private CommonJsService _CommonJs { get; set; } = default!;

        [Parameter, EditorRequired]
        public List<int> Numbers { get; set; } = default!;
        private List<string> _AvailableNumbers = new();

        private int _TargetNumber;
        private bool _ShowTargetNumber = false;
        private List<double> _CalculationResults = new();
        private IJSObjectReference _JsModule = default!;

        private string _CalculatorInputId = Guid.NewGuid().Short();
        private string _AudioId = Guid.NewGuid().Short();
        private string? _CalculatorInputValue { get; set; }
        private string? _LastInput;
        private char[] _AcceptedCharacters =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            '+', '-', '*', '/'
        };

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _TargetNumber = new Random().Next(101, 1000);
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _JsModule = await _Js.ImportJsModule();
        }

        private void InitializeAvailableNumbers()
        {
            foreach (var number in Numbers)
                _AvailableNumbers.Add(number.ToString());
        }

        private async void CalculatorInput_OnKeyUp(KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case "Enter":
                    if (_CalculatorInputValue == null)
                        break;

                    var numbers = _CalculatorInputValue.Split('+', '-', '*', '/')
                        .Where(x => !string.IsNullOrEmpty(x))
                        .ToList();

                    InitializeAvailableNumbers();

                    bool valid = true;
                    foreach (var number in numbers)
                    {
                        if (_AvailableNumbers.Contains(number))
                        {
                            _AvailableNumbers.Remove(number);
                        }
                        else
                        {
                            valid = false;
                            break;
                        }
                    }

                    if (!valid)
                    {
                        await SetCalculatorInputValue("");
                        await _CommonJs.PlayErrorSound();
                        return;
                    }

                    try
                    {
                        var result = new DataTable().Compute(_CalculatorInputValue, "");
                        _CalculationResults = _CalculationResults.Prepend(Math.Round(Convert.ToDouble(result), 3)).ToList();
                    }
                    catch (Exception)
                    {
                        // silly math
                    }
                    break;
            }
            StateHasChanged();
        }

        private async Task SetCalculatorInputValue(string value)
        {
            await _JsModule.InvokeVoidAsync("setCalculatorInputValue", value, _CalculatorInputId);
            _LastInput = value;
        }

        private async void DictionaryInput_OnInput(ChangeEventArgs e)
        {
            var value = (string)(e.Value ?? "");

            if (_LastInput == null) // First input
                _LastInput = "";

            if (Math.Abs(_LastInput.Count() - value.Count()) > 1)
            {
                // many paste or big delete we no like
                await SetCalculatorInputValue(_LastInput);
                return;
            }

            if (_LastInput.Count() > value.Count()) // Backspace
            {
                _LastInput = value;
                return;
            }

            var input = char.ToUpper(value.Last());

            string newInput;
            if (_AcceptedCharacters.Contains(input))
            {
                newInput = _LastInput + input;
            }
            else
            {
                newInput = _LastInput;
                await _CommonJs.PlayErrorSound();
            }

            await SetCalculatorInputValue(newInput);
        }
    }
}
