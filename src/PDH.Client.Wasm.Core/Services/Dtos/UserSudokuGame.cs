namespace PDH.Client.Wasm.Core.Services.Dtos;

public class UserSudokuGame
{
    public Guid Id { get; set; }

    public string? Name { get; set; }
    
    public int CellsLeft { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public DateTime UpdatedOn { get; set; }
}