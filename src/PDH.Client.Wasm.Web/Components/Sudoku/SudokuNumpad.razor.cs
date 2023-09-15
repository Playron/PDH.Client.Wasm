using Microsoft.AspNetCore.Components;
using PDH.Client.Wasm.Core.Services.Sudoku;

namespace PDH.Client.Wasm.Web.Components.Sudoku;

public partial class SudokuNumpad : ComponentBase
{
    [Parameter] public SudokuCell? SelectedCell { get; set; }
    
    [Parameter] public EventCallback<SudokuCell?> SelectedCellChanged { get; set; }

    private string CellIsSelected => SelectedCell is null ? 
        "display: flex; pointer-events: none; cursor: not-allowed; background-color: lightgray;"
        : "display: flex;";

    private int GetPositionValue(int i, int y) => (i, y) switch
    {
        (1, 1) => 1,
        (1, 2) => 2,
        (1, 3) => 3,
        (2, 1) => 4,
        (2, 2) => 5,
        (2, 3) => 6,
        (3, 1) => 7,
        (3, 2) => 8,
        (3, 3) => 9,
        _ => 0
    };

    private async Task ChangeCellValue(int positionValue)
    {
        SelectedCell!.Value = positionValue;
        await SelectedCellChanged.InvokeAsync(SelectedCell);
    }
}