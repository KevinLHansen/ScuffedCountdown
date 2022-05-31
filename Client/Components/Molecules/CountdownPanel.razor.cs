using BlazorComponentUtilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScuffedCountdown.Client.Components.Molecules
{
    public partial class CountdownPanel : ComponentBase
    {
        [Inject]
        private IJSRuntime _Js { get; set; } = default!;

        private string _AudioId = "countdown-sfx";
        private bool _IsPlaying = false;

        private IJSObjectReference _JsModule { get; set; } = default!;

        private string _TimerClasses => new CssBuilder()
            .AddClass("active", when: _IsPlaying)
            .Build();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _JsModule = await _Js.InvokeAsync<IJSObjectReference>("import", "./Components/Molecules/CountdownPanel.razor.js");
        }

        private async Task Timer_OnClick()
        {
            var playing = await _JsModule.InvokeAsync<bool>("handleMusic", _AudioId);
            _IsPlaying = playing;
        }
    }
}
