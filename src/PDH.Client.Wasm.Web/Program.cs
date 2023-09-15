using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PDH.Client.Wasm.Core.PDH.Services;
using PDH.Client.Wasm.Web;
using PDH.Client.Wasm.Web.Auth0AuthenticationStateProvider;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped<CustomAuthorizationMessageHandler>();

builder.Services
    .AddHttpClient<ClientService>("ClientService",
        client => client.BaseAddress = new Uri(builder.Configuration["Endpoints:ServicesApi"]!)
    )
    .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("ClientService"));


builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code";
    options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]);
}).AddAccountClaimsPrincipalFactory<AccountClaimsPrincipalFactory<RemoteUserAccount>>();

builder.Services
    .AddAuthorizationCore();

await builder.Build().RunAsync();