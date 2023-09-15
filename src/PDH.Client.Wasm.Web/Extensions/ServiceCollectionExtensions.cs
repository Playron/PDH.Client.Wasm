using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using MudBlazor;
using MudBlazor.Services;
using PDH.Client.Wasm.Core.HouseholdInventory;
using PDH.Client.Wasm.Core.PDH.Services;
using PDH.Client.Wasm.Core.Services;
using PDH.Client.Wasm.Core.Services.Sudoku;
using PDH.Client.Wasm.Web.Auth0AuthenticationStateProvider;
using Microsoft.Extensions.Configuration;
using PDH.Client.Wasm.Core.Services.Dtos;

namespace PDH.Client.Wasm.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddClientServices(this IServiceCollection services)
    {
        services.AddScoped<CustomAuthorizationMessageHandler>();
        services.AddSingleton<Config>();
        services.AddMudServices(opt =>
        {
            opt.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomCenter;
        });

        services.AddScoped<SudokuGameGenerator>();
    }

    public static void AddHttpClients(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddHttpClient<ClientService>("ClientService",
                client => client.BaseAddress = new Uri(config["Endpoints:ServicesApi"]!)
            ).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

        services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
            .CreateClient("ClientService"));
        
        services.AddHttpClient<HouseholdProductService>(options =>
        {
            options.BaseAddress = new Uri(config["Endpoints:KassalApi"]!);
            options.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", config["Keys:KassalToken"]);
        });
        
        services.AddHttpClient<SudokuService>(options =>
        {
            options.BaseAddress = new Uri(config["Endpoints:SudokuApi"]!);
        });
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