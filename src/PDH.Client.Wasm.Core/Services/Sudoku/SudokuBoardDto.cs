using PDH.Client.Wasm.Core.Services.Dtos;

namespace PDH.Client.Wasm.Core.Services.Sudoku;

public class Board
{
    public Newboard Newboard { get; set; } = new Newboard();

    public static SudokuBoard MapFromSavedBoard(SudokuGame game)
    {
        var board = new SudokuBoard();
        board.Id = game.Id;
        board.Name = game?.Name!;
        var array = new Cell[9, 9];
        for (int i = 0; i < 9; i++)
        {
            var index = 0;
            var skip = i * 9;
            var take = 9;
            IEnumerable<Cell> cells = game!.Cells!.Skip(skip).Take(take);
            foreach (var cell in cells)
            {
                array[i, index] = cell;
                index++;
            }
        }
        for (int i = 0; i <= 8; i++)
        {
            for (int y = 0; y <= 8; y++)
            {
                board.Cells[i, y].Value = array[i, y].Value;
                board.Cells[i, y].IsLocked = array[i, y].IsLocked;
                board.Cells[i, y].Id = array[i, y].Id;
            }
        }

        return board;
    }
    
    public static SudokuBoard ToSolvable(Board board)
    {
        var sudokuBoard = new SudokuBoard();
        sudokuBoard.Id = null;
        foreach (var grid in board.Newboard.Grids)
        {
            var sudokuRow = 0;
            var sudokuCol = 0;
            ToUnfinishedBoard(grid, sudokuBoard, sudokuRow, sudokuCol);
        }

        return sudokuBoard;
    }
    
    public static SudokuBoard ToSolvable(SudokuBoard board)
    {
        foreach (var cell in board.Cells)
        {
            if (!cell.IsLocked)
                cell.Value = 0;
        }

        return board;
    }
    public static SudokuBoard ToSolution(Board board)
    {
        var sudokuBoard = new SudokuBoard();
        foreach (var grid in board.Newboard.Grids)
        {
            var sudokuRow = 0;
            var sudokuCol = 0;
            ToSolutionBoard(grid, sudokuBoard, sudokuRow, sudokuCol);
        }

        return sudokuBoard;
    }
    

    private static void ToUnfinishedBoard(Grid grid, SudokuBoard sudokuBoard, int sudokuRow, int sudokuCol)
    {
        foreach (var row in grid.Value)
        {
            foreach (var value in row)
            {
                var cell = sudokuBoard.Cells[sudokuRow, sudokuCol];
                cell.Value = value;
                cell.Id = null;
                if (!value.Equals((0)))
                {
                    cell.IsLocked = true;
                }

                sudokuCol++;
            }

            sudokuCol = 0;
            sudokuRow++;
        }
    }
    private static void ToSolutionBoard(Grid grid, SudokuBoard sudokuBoard, int sudokuRow, int sudokuCol)
    {
        foreach (var row in grid.Solution)
        {
            foreach (var value in row)
            {
                var cell = sudokuBoard.Cells[sudokuRow, sudokuCol];
                cell.Value = value;
                sudokuCol++;
            }

            sudokuCol = 0;
            sudokuRow++;
        }
    }
}

public class Newboard
{
    public List<Grid> Grids { get; set; } = null!;
    public int Results { get; set; }
    public string Message { get; set; } = null!;
}
public class Grid
{
    public List<List<int>> Value { get; set; } = null!;
    public List<List<int>> Solution { get; set; } = null!;
    public string Difficulty { get; set; } = null!;
}
