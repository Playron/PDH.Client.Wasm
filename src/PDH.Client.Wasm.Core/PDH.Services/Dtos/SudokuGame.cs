namespace PDH.Client.Wasm.Core.PDH.Services.Dtos;

public class SudokuGame
{
    public Guid Id { get; set; }
    
    public string? Name { get; set; }
    public string? UserId { get; set; }
    
    public IEnumerable<Cell>? Cells { get; set; }
}

public class Cell
{
    public Guid Id { get; init; }
    
    public int Value { get; init; }

    public bool IsLocked { get; init; }
}

