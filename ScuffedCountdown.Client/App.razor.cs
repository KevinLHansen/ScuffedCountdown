using Microsoft.AspNetCore.Components;
using ScuffedCountdown.Client.Services;

namespace ScuffedCountdown.Client
{
    public partial class App : ComponentBase
    {
        [Inject]
        private StateManager _StateManager { get; set; } = default!;
        [Inject]
        private ICountdownManager _CountdownManager { get; set; } = default!;
        [Inject]
        private CommonJsService _CommonJs { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await _StateManager.Initalize();
            await _CountdownManager.Initialize();
            var state = await _StateManager.GetState();
            await HandleState(state);

            await base.OnInitializedAsync();
        }

        private async Task HandleState(ScuffedCountdownState state)
        {
            var themeColor = state.UserSettings.ThemeColor;
            if (themeColor != null)
                await _CommonJs.SetMasterColor(themeColor.H, themeColor.S);
        }
    }
}
