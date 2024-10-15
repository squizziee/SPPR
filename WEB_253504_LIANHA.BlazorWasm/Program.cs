using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WEB_253504_LIANHA.BlazorWasm;
using WEB_253504_LIANHA.BlazorWasm.Services;

//var builder = WebAssemblyHostBuilder.CreateDefault(args);
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var uriData = new UriData
{
    ApiUri = builder.Configuration.GetSection("UriData").GetValue<string>("ApiUri")!
};

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services
    .AddHttpClient<IDataService, ApiDataService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

//builder.Services.AddScoped<IDataService, ApiDataService>();

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Keycloak", options.ProviderOptions);
});

await builder.Build().RunAsync();
