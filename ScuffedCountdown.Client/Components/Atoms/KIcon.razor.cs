using BlazorComponentUtilities;
using Microsoft.AspNetCore.Components;

namespace ScuffedCountdown.Client.Components.Atoms
{
    public partial class KIcon : ComponentBase
    {
        [Parameter]
        public string IconName { get; set; } = "code";
        [Parameter]
        public int Size { get; set; } = 16;

        private string CssClasses => new CssBuilder()
            .AddClass("fa-solid")
            .AddClass($"fa-{IconName}")
            .Build();

        private string CssStyles => new StyleBuilder()
            .AddStyle("width", $"{Size}px")
            .AddStyle("height", $"{Size}px")
            .AddStyle("font-size", $"{Math.Floor(Size * 0.9)}px")
            .Build();
    }
}
