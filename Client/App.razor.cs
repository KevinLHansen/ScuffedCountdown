using Microsoft.AspNetCore.Components;
using ScuffedCountdown.Client.Services;

namespace ScuffedCountdown.Client
{
    public partial class App : ComponentBase
    {
        [Inject]
        private StateManager _StateManager { get; set; } = default!;
        [Inject]
        private CommonJsService _CommonJs { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var state = await _StateManager.GetState();
            await HandleState(state);
        }

        private async Task HandleState(ScuffedCountdownState state)
        {
            var themeColor = state.UserSettings.ThemeColor;
            if (themeColor != null)
                await _CommonJs.SetMasterColor(themeColor.H, themeColor.S);
        }
    }
}
