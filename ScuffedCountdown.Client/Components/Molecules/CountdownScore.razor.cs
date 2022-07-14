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

        protected override async Task OnInitializedAsync()
        {
            await UpdateScore();
            await base.OnInitializedAsync();
        }

        private int _Score = 0;

        private async Task SubtractScore()
        {
            await _CountdownManager.SubtractScore(Team, 1);
            await UpdateScore();
        }

        private async Task AddScore()
        {
            await _CountdownManager.AddScore(Team, 1);
            await UpdateScore();
        }

        private async Task UpdateScore()
        {
            _Score = await _CountdownManager.GetScore(Team);
        }
    }
}
