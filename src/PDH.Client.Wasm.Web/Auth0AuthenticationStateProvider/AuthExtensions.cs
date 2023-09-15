using Microsoft.AspNetCore.Components.Authorization;

namespace PDH.Client.Wasm.Web.Auth0AuthenticationStateProvider;

public static class AuthExtensions
{
    public static async Task<string?> GetUserId(this AuthenticationStateProvider? provider)
    {
        if (provider != null)
        {
            var authState = await provider
                .GetAuthenticationStateAsync();
            var user = authState.User;
            return user.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
        }

        return string.Empty;
    }
}