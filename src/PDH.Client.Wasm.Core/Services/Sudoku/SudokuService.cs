using System.Net.Http.Json;

namespace PDH.Client.Wasm.Core.Services.Sudoku;

public class SudokuService
{
    private readonly HttpClient _client;

    public SudokuService(HttpClient client)
    {
        _client = client;
    }

    public async Task<Board> GetSudokuPuzzle()
    {
        var result = await _client.GetFromJsonAsync<Board>("api/dosuku");
        if (result is null)
        {
            return new Board();
        }
        return result;
    } 
}