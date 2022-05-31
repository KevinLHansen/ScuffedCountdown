using BlazorComponentUtilities;
using Microsoft.AspNetCore.Components;

namespace ScuffedCountdown.Client.Components.Atoms
{
    public partial class KIconButton : ComponentBase
    {
        [Parameter]
        public EventCallback OnClick { get; set; }
        [Parameter]
        public string IconName { get; set; } = "code";
        [Parameter]
        public KIconButtonSize Size { get; set; } = KIconButtonSize.Small;
        [Parameter]
        public KIconButtonStyle Style { get; set; } = KIconButtonStyle.Dark;

        private string _CssClasses => new CssBuilder()
            .AddClass(Enum.GetName(Style))
            .Build().ToLower();

        private int _Size => Size switch
        {
            KIconButtonSize.Small => 16,
            KIconButtonSize.Medium => 32,
            KIconButtonSize.Large => 64,
            _ => 16
        };
}

    public enum KIconButtonSize
    {
        Small,
        Medium,
        Large
    }

    public enum KIconButtonStyle
    {
        Light,
        Dark
    }
}
