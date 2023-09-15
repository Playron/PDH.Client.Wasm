namespace PDH.Client.Wasm.Core.PDH.Services.Dtos;

public class TicTacToeGame
{
    public Guid Id { get; set; }
    public string? UserId { get; set; }
    
    public string? CompetitorUserId { get; set; }
    public DateTime CreatedOn { get; set; } 
}