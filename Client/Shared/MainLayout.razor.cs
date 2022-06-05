using BlazorComponentUtilities;
using ColorHelper;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ScuffedCountdown.Client.Services;

namespace ScuffedCountdown.Client.Shared
{
    public partial class MainLayout
    {
        [Inject]
        private CommonJsService _CommonJs { get; set; } = default!;
        [Inject]
        private StateManager _StateManager { get; set; } = default!;
        [Inject]
        private IJSRuntime _JsRuntime { get; set; } = default;

        private IJSObjectReference _Js { get; set; } = default;

        private bool _SettingsVisible = false;
        private string _ColorPickerInputId = Guid.NewGuid().ToString();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _Js = await _JsRuntime.InvokeAsync<IJSObjectReference>("import", $"./Shared/MainLayout.razor.js");
            await SetColorPickerInputValue();
        }

        private async Task SetColorPickerInputValue()
        {
            var themeColor = (await _StateManager.GetState()).UserSettings.ThemeColor;
            await _Js.InvokeVoidAsync("setColorInputValue", _ColorPickerInputId, $"#{ColorConverter.HslToHex(themeColor)}");
        }

        private string _SettingsButtonCssClasses => new CssBuilder()
            .AddClass("active", when: _SettingsVisible)
            .Build();

        private string _SettingsCssClasses => new CssBuilder()
            .AddClass("visible", when: _SettingsVisible)
            .Build();

        private void _SettingsButton_OnClick()
        {
            _SettingsVisible = !_SettingsVisible;
        }

        private async Task ColorPickerButton_OnClick()
        {
            await _CommonJs.ClickElement(_ColorPickerInputId);
        }

        private async Task ColorPicker_OnChange(ChangeEventArgs e)
        {
            var color = e.Value;
            if (color == null)
                return;

            var hsl = ColorConverter.HexToHsl(new HEX((string)color));
            await _CommonJs.SetMasterColor(hsl.H, hsl.S);

            await _StateManager.ModifyState(stateModifer =>
            {
                stateModifer.UserSettings.ThemeColor = hsl;
            });
        }
    }
}
