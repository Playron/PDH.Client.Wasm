using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using MudBlazor.Services;
using PDH.Client.Wasm.Core.PDH.Services;
using PDH.Client.Wasm.Web.Auth0AuthenticationStateProvider;

namespace PDH.Client.Wasm.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddClientServices(this IServiceCollection services)
    {
        services.AddScoped<CustomAuthorizationMessageHandler>();
        services.AddMudServices();
    }

    public static void AddHttpClients(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddHttpClient<ClientService>("ClientService",
                client => client.BaseAddress = new Uri(config["Endpoints:ServicesApi"]!)
            ).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

        services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
            .CreateClient("ClientService"));
    }
    

    public static void AddAuth(this IServiceCollection services, IConfiguration config)
    {
        services.AddOidcAuthentication(options =>
        {
            config.Bind("Auth0", options.ProviderOptions);
            options.ProviderOptions.ResponseType = "code";
            options.ProviderOptions.AdditionalProviderParameters.Add("audience", config["Auth0:Audience"]);
        }).AddAccountClaimsPrincipalFactory<AccountClaimsPrincipalFactory<RemoteUserAccount>>();

        services.AddAuthorizationCore();
    }
}