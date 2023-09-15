using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using PDH.Client.Wasm.Core.PDH.Services;
using PDH.Client.Wasm.Core.Services;
using PDH.Client.Wasm.Core.Services.Sudoku;
using PDH.Client.Wasm.Web.Auth0AuthenticationStateProvider;
using PDH.Client.Wasm.Web.Components.Sudoku;
using SudokuCell = PDH.Client.Wasm.Core.Services.Sudoku.SudokuCell;


namespace PDH.Client.Wasm.Web.Pages;

public partial class Sudoku : ComponentBase
{
    [Inject] private AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    
    [Inject] private SudokuService SudokuService { get; set; } = null!;

    [Inject] private IDialogService DialogService { get; set; } = null!; 
    
    [Inject] private ClientService ClientService { get; set; } = null!;
    
    [Inject] private SudokuGameGenerator GameGenerator { get; set; } = null!;

    private bool IsLoading { get; set; } = false;
    
    private Board? RetrievedBoard { get; set; }
    
    private SudokuBoard? Board { get; set; }
    
    private SudokuCell? SelectedCell { get; set; }

    private Guid GameId { get; set; }
    
    private string? ClientId { get; set; }

    private Stack<SudokuCell>? History { get; set; } = new Stack<SudokuCell>();
    
    private Stack<SudokuCell>? Undos { get; set; } = new Stack<SudokuCell>();
    
    private IDialogReference? Dialog { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        ClientId = await AuthenticationStateProvider.GetUserId();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await OpenGameTypeDialog();
        }
    }

    private async Task<SudokuBoard?> OpenGameTypeDialog()
    {
        var parameters = new DialogParameters
        {
            ["ClientId"] = ClientId,
            ["GameTypeChanged"] = EventCallback.Factory
                .Create<(SudokuGameTypeEnum type, Guid? id)>(this,
                    async args => await StartGame(args.type, args.id)),
            ["DeleteSave"] = EventCallback.Factory
                .Create<(Guid id, string saveName)>(this,
                    async args => await DeleteGaveSave(args.id, args.saveName))
        };
        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true
        };
        Dialog = await DialogService.ShowAsync<ChooseSudokuGameTypeDialog>("Hva ønsker du å spille?", parameters, options);
        return Board;
    }

    private async Task DeleteGaveSave(Guid id, string saveName)
    {
        bool? result = await DialogService.ShowMessageBox(
            $"Do you want to delete {saveName}", 
            "This can't be undone", 
            yesText:"Delete", cancelText:"Cancel");
        if (result is not null)
        {
            var message = await ClientService.DeleteSavedGame(id);
            Snackbar.Add(message.Value, Severity.Success);
        }
        await InvokeAsync(StateHasChanged);
    }

    private async Task StartGame(SudokuGameTypeEnum gameType, Guid? id)
    {
        IsLoading = true;
        Dialog!.Close();
        Board = gameType switch
        {
            SudokuGameTypeEnum.NewGame => await NewGame(),
            SudokuGameTypeEnum.SavedGame => await RetrieveSavedGame(id!.Value),
            SudokuGameTypeEnum.None => await NewGame(),
            _ => await NewGame()
        };
        IsLoading = false;
    }
    
    private async Task<SudokuBoard> NewGame()
    {
        SelectedCell = null;
        RetrievedBoard = await SudokuService.GetSudokuPuzzle();
        return PDH.Client.Wasm.Core.Services.Sudoku.Board.ToSolvable(RetrievedBoard);
    }

    private async Task<SudokuBoard?> RetrieveSavedGame(Guid id)
    {
        var board = await ClientService.GetUserSavedGame(id, ClientId!);
        return Core.Services.Sudoku.Board.MapFromSavedBoard(board!);
    }
    
    private async Task PerformBoardAction(BoardAction action)
    {
        SelectedCell = null;
        Board = action switch
        {
            BoardAction.NewGame => await NewGame(),
            BoardAction.GetSolution => Core.Services.Sudoku.Board.ToSolution(RetrievedBoard!),
            BoardAction.Reset => Core.Services.Sudoku.Board.ToSolvable(Board!),
            BoardAction.Solve => GameGenerator.SolveBoard(Board!),
            BoardAction.Undo => UndoLastMove(),
            BoardAction.Redo => RedoLastMove(),
            BoardAction.BackToUserSaves => await OpenGameTypeDialog(),
            _ => new SudokuBoard()
        };
        IsLoading = false;
        await InvokeAsync(StateHasChanged);
    }

    private SudokuBoard? RedoLastMove()
    {
        if (Undos!.Any())
        {
            var redoCell = Undos!.Pop();
            History?.Push(new SudokuCell
            {
                Id = redoCell.Id,
                Value = redoCell.Value,
                IsLocked = redoCell.IsLocked,
                Placement = redoCell.Placement
            });
            Board!.Cells[redoCell.Placement.Row, redoCell.Placement.Column].Value = redoCell.Value;
        }
        return Board;
    }

    private SudokuBoard? UndoLastMove()
    {
        if (History!.Count > 0)
        {
            var undoCell = History!.Pop();
            Undos?.Push(new SudokuCell
            {
                Id = undoCell.Id,
                Value = undoCell.Value,
                IsLocked = undoCell.IsLocked,
                Placement = undoCell.Placement
            });
            Board!.Cells[undoCell.Placement.Row, undoCell.Placement.Column].Value = 0;
        }
        return Board;
    }

    private async Task<SudokuBoard> SaveSudokuBoard(string name)
    {
        var command = Board?.MapFromBoardToDto(ClientId!, name);
        try
        {
            await ClientService.SaveSudokuGame(command!);
            Snackbar.Add("Gave was updates successfully");
        }
        catch (Exception)
        {
            Snackbar.Add("something went wrong", Severity.Error);
        }
        return Board!;
    }

    private void SetNewValueAndAddToHistory(SudokuCell? cell)
    {
        History!.Push(cell!);
        SelectedCell = null;
    }
}