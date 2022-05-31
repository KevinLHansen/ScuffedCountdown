using BlazorComponentUtilities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScuffedCountdown.Client.Components.Atoms
{
    public partial class KButton : ComponentBase
    {
        [Parameter]
        public EventCallback OnClick { get; set; }
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
        [Parameter]
        public KButtonStyle Style { get; set; } = KButtonStyle.Light;

        private string _CssClasses => new CssBuilder()
            .AddClass(Enum.GetName(Style))
            .Build().ToLower();
    }

    public enum KButtonStyle
    {
        Light,
        Dark
    }
}
