using System.Net.Http.Headers;
using System.Text.Json;
using PDH.Client.Wasm.Core.Services.Dtos;

namespace PDH.Client.Wasm.Core.HouseholdInventory;

public class HouseholdProductService
{
    private readonly HttpClient _client;
    private readonly Config _config;

    private readonly JsonSerializerOptions? _serializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
    public HouseholdProductService(HttpClient client, Config config)
    {
        _client = client;
        _config = config;
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _config.Properties["KassalToken"]);
    }

    public async Task<ExternalProductDto> SearchForHouseholdProductAsync(string term)
    {
        var response = await _client.GetAsync($"products?search={term}");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<ExternalProductDto>(json, _serializerOptions);
        if (result != null)
        {
            return result;
        }

        return new ExternalProductDto();
    }
}