using Microsoft.AspNetCore.Components;
using ScuffedCountdown.Client.Services;

namespace ScuffedCountdown.Client.Components.Molecules
{
    public partial class CountdownScore : ComponentBase
    {
        [Inject]
        private ICountdownManager _CountdownManager { get; set; } = default!;

        [Parameter]
        public string Team { get; set; } = string.Empty;

        private int _Score => _CountdownManager.GetScore(Team);

        private void SubtractScore()
        {
            _CountdownManager.SubtractScore(Team, 1);
        }

        private void AddScore()
        {
            _CountdownManager.AddScore(Team, 1);
        }
    }
}
