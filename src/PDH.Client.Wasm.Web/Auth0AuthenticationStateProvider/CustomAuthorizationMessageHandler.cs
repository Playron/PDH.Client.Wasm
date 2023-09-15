using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace PDH.Client.Wasm.Web.Auth0AuthenticationStateProvider;

public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public CustomAuthorizationMessageHandler(
        IAccessTokenProvider provider,
        NavigationManager navigation,
        IConfiguration configuration)
        : base(provider, navigation)
    {
        ConfigureHandler(authorizedUrls: new[] { configuration["Auth0:Audience"] });
    }
}