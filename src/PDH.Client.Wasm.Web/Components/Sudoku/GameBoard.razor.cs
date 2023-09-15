using Microsoft.AspNetCore.Components;
using PDH.Client.Wasm.Core.PDH.Services;
using PDH.Client.Wasm.Core.Services;
using PDH.Client.Wasm.Core.Services.Sudoku;
using SudokuGameGenerator = PDH.Client.Wasm.Core.Services.Sudoku.SudokuGameGenerator;

namespace PDH.Client.Wasm.Web.Components.Sudoku;

public partial class GameBoard : ComponentBase
{
    [Inject] private SudokuGameGenerator GameGenerator { get; set; } = null!;

    [Inject] private ClientService ClientService { get; set; } = null!;
    
    [Parameter] public EventCallback<SudokuCell> SelectedCellChanged { get; set; }
    
    [Parameter] public EventCallback<BoardAction> BoardActionChanged { get; set; }
    
    [Parameter] public EventCallback<string> SaveGameChanged  { get; set; }
    
    [Parameter] public SudokuCell? SelectedCell { get; set; }

    [Parameter] public SudokuBoard? Board { get; set; }
    
    [Parameter] public string? ClientId { get; set; }
    
    [Parameter] public Guid GameId { get; set; }
    
    private string? SaveName { get; set; }

    private bool IsValidated => GameGenerator.ValidateBoard(Board!);
    
    private bool IsFindingSolution { get; set; }
    
    private string GetBoldLines(int row, string lineWeight) => row switch
    {
        0 => $"border-top: {lineWeight} solid black;",
        2 => $"border-bottom: {lineWeight} solid black;",
        5 => $"border-bottom: {lineWeight} solid black;",
        8 => $"border-bottom: {lineWeight} solid black;",
        _ => string.Empty
    };

    private string GetBoldCols(int col, string lineWeight) => col switch
    {
        0 => $"border-left: {lineWeight} solid black;",
        2 => $"border-right: {lineWeight} solid black;",
        5 => $"border-right: {lineWeight} solid black;",
        8 => $"border-right: {lineWeight} solid black;",
        _ => string.Empty
    };

    private string CellStyle(SudokuCell cell, SudokuCell? selectedCell) => cell switch
    {
        { IsLocked: true , Value: > 0}  => "background-color: #E8E8E8 !important;",
        _ when selectedCell == cell  => "background-color: #99ff99 !important;", 
        { IsLocked: false, Value: > 0 } => "background-color: #FFFFE0",
        _ when selectedCell is null  => "background-color: white",
        _ => "background-color: white"
    };

    private async Task SetSelectedCell(SudokuCell? cell)
    {
        if (cell is { IsLocked: false })
        {
            SelectedCell = SelectedCell == cell ? null : cell;
            await SelectedCellChanged.InvokeAsync(SelectedCell);
        }
        else
        {
            SelectedCell = null;
            await SelectedCellChanged.InvokeAsync(SelectedCell);
        }
        await InvokeAsync(StateHasChanged);
    }

    public async Task BoardActionHasHappened(BoardAction action)
    {
        await BoardActionChanged.InvokeAsync(action);
    }
    
    private string IsSolved()
    {
        return IsValidated ? "solved" : "unsolved";
    }
}

public enum BoardAction
{
    NewGame,
    Reset,
    GetSolution,
    Solve,
    Save,
    Undo,
    Redo,
    BackToUserSaves
}