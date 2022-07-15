using BlazorComponentUtilities;
using ColorHelper;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ScuffedCountdown.Client.Extensions;
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
        private IJSRuntime _JsRuntime { get; set; } = default!;

        private IJSObjectReference _Js { get; set; } = default!;

        private bool _SettingsVisible = false;
        private string _ColorPickerInputId = Guid.NewGuid().Short();
        private string _FreeDictionaryCheckboxId = Guid.NewGuid().Short();
        private string _UrbanDictionaryCheckboxId = Guid.NewGuid().Short();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _Js = await _JsRuntime.InvokeAsync<IJSObjectReference>("import", $"./Shared/MainLayout.razor.js");
            await SetColorPickerInputValue();
            await SetCheckboxStates();
        }

        private async Task SetColorPickerInputValue()
        {
            var themeColor = (await _StateManager.GetState()).UserSettings.ThemeColor;
            if (themeColor != null)
                await _Js.InvokeVoidAsync("setColorInputValue", _ColorPickerInputId, $"#{ColorConverter.HslToHex(themeColor)}");
        }

        private async Task SetCheckboxStates()
        {
            var userSettings = (await _StateManager.GetState()).UserSettings;
            await _Js.InvokeVoidAsync("setCheckboxState", _FreeDictionaryCheckboxId, userSettings.UseFreeDictionary);
            await _Js.InvokeVoidAsync("setCheckboxState", _UrbanDictionaryCheckboxId, userSettings.UseUrbanDictionary);
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

        private async void ColorPickerButton_OnClick()
        {
            await _CommonJs.ClickElement(_ColorPickerInputId);
        }

        private async void ColorPicker_OnChange(ChangeEventArgs e)
        {
            var color = e.Value;
            if (color == null)
                return;

            var hsl = ColorConverter.HexToHsl(new HEX((string)color));
            await _CommonJs.SetMasterColor(hsl.H, hsl.S);

            await _StateManager.ModifyState(state =>
            {
                state.UserSettings.ThemeColor = hsl;
            });
        }

        private async void FreeDictionaryCheckbox_OnChange(ChangeEventArgs e)
        {
            if (e.Value == null)
                return;

            var useFreeDictionary = (bool)e.Value;
            await _StateManager.ModifyState(state =>
            {
                state.UserSettings.UseFreeDictionary = useFreeDictionary;
            });
        }

        private async void UrbanDictionaryCheckbox_OnChange(ChangeEventArgs e)
        {
            if (e.Value == null)
                return;

            var useUrbanDictionary = (bool)e.Value;
            await _StateManager.ModifyState(state =>
            {
                state.UserSettings.UseUrbanDictionary = useUrbanDictionary;
            });
        }
    }
}
