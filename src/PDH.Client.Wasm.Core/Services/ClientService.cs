using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PDH.Client.Wasm.Core.PDH.Services;
using PDH.Client.Wasm.Core.Services.Dtos;
using System;  
using System.Configuration;
using Microsoft.Extensions.Configuration.Memory;

namespace PDH.Client.Wasm.Core.Services;

public class ClientService
{
    private readonly HttpClient _client;
    private Config _config;

    private readonly JsonSerializerOptions? _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ClientService(HttpClient client, Config config)
    {
        _client = client;
        _config = config;
    }

    public async Task<IEnumerable<TechnicalSkill>?> GetAllTechnicalSkills()
    {
        var response = await _client.GetAsync("technicalSkills");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<IEnumerable<TechnicalSkill>>(json, _serializerOptions);
    }

    public async Task<TechnicalSkill?> AddNewTechnicalSkill(AddNewTechnicalSkillCommand command)
    {
        var response = await _client.PostAsJsonAsync("technicalSkills", command);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<TechnicalSkill>(json, _serializerOptions);
    }

    public async Task<LanguageWorkAndTechnicalList?> GetAllLists()
    {
        var response = await _client.GetAsync("allLists");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<LanguageWorkAndTechnicalList>(json, _serializerOptions);
    }

    public async Task SaveSudokuGame(SaveSudokuBoardCommand command)
    {
        var response = await _client.PostAsJsonAsync("sudokuGame", command);

        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<UserSudokuGame>?> GetUserSavedGames(string userId)
    {
        var response = await _client.GetAsync($"sudokuGame/{userId}");

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<IEnumerable<UserSudokuGame>?>(json, _serializerOptions);
    }

    public async Task<SudokuGame?> GetUserSavedGame(Guid id, string userId)
    {
        var response = await _client.GetAsync($"sudokuGame/{id}/user/{userId}");

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<SudokuGame>(json, _serializerOptions);
    }

    public async Task<ApiMessage> DeleteSavedGame(Guid id)
    {
        var response = await _client.DeleteAsync($"sudokuGame/{id}");

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<ApiMessage>(json, _serializerOptions)!;
    }

    public async Task<IEnumerable<TicTacToeGame>?> GetOpenTicTacToeGames()
    {
        var response = await _client.GetAsync("ticTacToe");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<IEnumerable<TicTacToeGame>>(json, _serializerOptions);
    }

    public async Task<Message?> JoinTicTacToeGames(Guid gameId, JoinTicTacToeGameDto joinGame)
    {
        var response = await _client.PostAsJsonAsync($"ticTacToe/{gameId}", joinGame);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<Message>(json, _serializerOptions);
    }

    public async Task<IEnumerable<HouseholdProductDto>?> GetAllHouseholdProducts()
    {
        var response = await _client.GetAsync("householdProducts");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<RootHouseholdProductDto>(json, _serializerOptions);
        return result.HouseholdProducts;
    }

    public async Task<HttpResponseMessage> AddHouseholdProduct(HouseholdProductDto product)
    {
        var response = await _client.PostAsJsonAsync("householdProducts", product);
        return response.EnsureSuccessStatusCode();
    }

    public async Task<HttpResponseMessage> DeleteHouseholdProduct(Guid id)
    {
        var response = await _client.DeleteAsync($"householdProduct/{id}");

        return response.EnsureSuccessStatusCode();
    }

    public async Task<HttpResponseMessage> UpdateHouseholdProduct(Guid id, HouseholdProductDto dto)
    {
        var response = await _client.PostAsJsonAsync($"householdProduct/{id}", dto);

        return response.EnsureSuccessStatusCode();
    }
    
    public async Task<Dictionary<string, string>> GetConfig()
    {
        var response = await _client.GetAsync("config");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        var config = JsonSerializer.Deserialize<Dictionary<string, String>>(result);
        _config.Properties = config!;
        
        return config!;
    }
}

public class Message
{
    public string? Value { get; set; }
}

public class JoinTicTacToeGameDto
{
    public string UserId { get; set; } = null!;
}

public class ApiMessage
{
    public int Id { get; set; }
    public string? Value { get; set; }
}