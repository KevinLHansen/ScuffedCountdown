using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ScuffedCountdown.Client;
using ScuffedCountdown.Client.Services;
using ScuffedCountdown.Client.APIs;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var services = builder.Services;

services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// External packages
services
    .AddBlazoredLocalStorage();

// Internal services
services
    .AddScoped<ICountdownManager, CountdownManager>()
    .AddScoped<CommonJsService>()
    .AddScoped<StateManager>();

// APIs
services
    .AddRefitClient<IFreeDictionaryApi>()
    .ConfigureHttpClient(conf =>
    {
        conf.BaseAddress = IFreeDictionaryApi.BaseUri;
    });

await builder.Build().RunAsync();
