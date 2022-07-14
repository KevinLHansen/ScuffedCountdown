using BlazorComponentUtilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScuffedCountdown.Client.Components.Atoms
{
    public partial class KNavLink : ComponentBase
    {
        [Inject]
        private NavigationManager _NavigationManager { get; set; } = default!;

        [Parameter]
        public string Text { get; set; } = "?";
        [Parameter]
        public string NavigationString { get; set; } = "index";

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _NavigationManager.LocationChanged += (object? sender, LocationChangedEventArgs e) =>
            {
                StateHasChanged();
            };
        }

        private bool IsCurrentPage(string location)
        {
            return location.Contains($"/{NavigationString}");
        }

        private void OnClick()
        {
            _NavigationManager.NavigateTo(NavigationString);
        }

        private string CssClasses => new CssBuilder()
            .AddClass("active", when: IsCurrentPage(_NavigationManager.Uri))
            .Build();
    }
}
