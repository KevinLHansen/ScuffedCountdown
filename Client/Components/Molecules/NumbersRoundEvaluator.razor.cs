using Microsoft.AspNetCore.Components;
using System.Data;

namespace ScuffedCountdown.Client.Components.Molecules
{
    public partial class NumbersRoundEvaluator : ComponentBase
    {
        [Parameter, EditorRequired]
        public List<int> Numbers { get; set; } = default!;

        private int _TargetNumber;
        private bool _ShowTargetNumber = false;

        private List<double> _CalculationResults = new();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _TargetNumber = new Random().Next(101, 1000);
        }

        private void Calculator_OnChange(ChangeEventArgs e)
        {
            try
            {
                var result = new DataTable().Compute(e.Value?.ToString(), "");
                _CalculationResults = _CalculationResults.Prepend(Math.Round(Convert.ToDouble(result), 3)).ToList();
            }
            catch (Exception)
            {
                // silly math
            }
        }
    }
}
