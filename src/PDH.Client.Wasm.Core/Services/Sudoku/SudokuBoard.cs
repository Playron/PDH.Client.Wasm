using PDH.Client.Wasm.Core.PDH.Services;
using PDH.Client.Wasm.Core.Services.Dtos;

namespace PDH.Client.Wasm.Core.Services.Sudoku;

public class SudokuBoard
{
    public Guid? Id { get; set; }
    public SudokuCell[,] Cells { get; set; } = new SudokuCell[9, 9];
    
    public string? Name { get; set; }
    public Dictionary<int, List<SudokuCell>> Quadrants { get; set; } = new();
    
    public SudokuBoard()
    {
        for (int i = 0; i <= 8; i++)
        {
            Quadrants.Add(i, new List<SudokuCell>());
        }

        for (int i = 0; i <= 8; i++)
        {
            for (int y = 0; y <= 8; y++)
            {
                Cells[i, y] = new SudokuCell(i,y);
                Quadrants[Cells[i, y].Placement.Quadrant].Add(Cells[i, y]);
            }
        }
    }

    public SaveSudokuBoardCommand MapFromBoardToDto(string userName, string gameName)
    {
        var cells = new List<Cell>();
        for (int x = 0; x <= 8; x++)
        {
            for (int y = 0; y <= 8; y++)
            {
                var cell = Cells[x, y];
                cells.Add(new Cell()
                {
                    Id = cell.Id ?? Guid.Empty,
                    Value = cell.Value,
                    IsLocked = cell.IsLocked,
                });
            }
        }
        return new SaveSudokuBoardCommand(new SudokuGame
        {
            Id = Id ?? Guid.Empty,
            UserId = userName,
            Cells = cells,
            Name = gameName
        });
    }
}

public class SudokuCell
{
    public Guid? Id { get; set; }
    
    public int Value { get; set; }
    
    public Placement Placement { get; set; } = null!;

    public bool IsLocked { get; set; }

    public SudokuCell(int row, int col)
    {
        Value = 0;
        Placement = new Placement
        {
            Row = row,
            Column = col,
        };
    }

    public SudokuCell() { }
}

public class Placement
{
    public int Row { get; set; }
    
    public int Column { get; set; }

    public int Quadrant => FindQuadrant(Row, Column);
    
    private int FindQuadrant(int row, int col)
    {
        return (row, col) switch
        {
            (<= 2, <= 2) => 0,
            (<= 2, <= 5) => 1,
            (<= 2, <= 8) => 2,
            (<= 5, <= 2) => 3,
            (<= 5, <= 5) => 4,
            (<= 5, <= 8) => 5,
            (<= 8, <= 2) => 6,
            (<= 8, <= 5) => 7,
            _ => 8
        };
    }
}
