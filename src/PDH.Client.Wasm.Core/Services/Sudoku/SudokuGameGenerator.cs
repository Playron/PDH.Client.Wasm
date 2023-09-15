namespace PDH.Client.Wasm.Core.Services.Sudoku;

public class SudokuGameGenerator
{
    private readonly IEnumerable<int> _possibleValues = new[]
        { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    
    public bool ValidateBoard(SudokuBoard board)
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (!ValidateFullBoard(board, board.Cells[row, col]))
                    return false;
            }
        }

        return true;
    }

    private bool ValidateFullBoard(SudokuBoard board, SudokuCell cell)
    {
        if (!IsCellInRowValid(board, cell.Placement.Row)) return false;
        if (!IsCellInColValid(board, cell.Placement.Column)) return false;
        if (!IsCellInQuadrantValid(board, cell.Placement.Quadrant)) return false;
        return true;
    }

    private bool IsCellInRowValid(SudokuBoard board, int row)
    {
        HashSet<int> values = new HashSet<int>();
        for (int i = 0; i < 9; i++)
        {
            var cellValue = board.Cells[row, i].Value;
            if (values.Contains(cellValue))
            {
                return false;
            }
            values.Add(cellValue);
        }
        return true;
    }

    private bool IsCellInColValid(SudokuBoard board, int col)
    {
        HashSet<int> values = new HashSet<int>();
        for (int i = 0; i < 9; i++)
        {
            var cellValue = board.Cells[i, col].Value;
            if (values.Contains(cellValue))
            {
                return false;
            }
            values.Add(cellValue);
        }
        return true;
    }

    private bool IsCellInQuadrantValid(SudokuBoard board, int quadrant)
    {
        var values = board.Quadrants[quadrant];
        if (values.Count != values.Distinct().Count())
        {
            return false;
        }

        return true;
    }

    private bool Validate(SudokuBoard board, int row, int col, int value)
    {
        var currentCell = board.Cells[row, col];
        if (!IsRowValid(board, currentCell, value)) return false;
        if (!IsColValid(board, currentCell, value)) return false;
        if (!IsQuadrantValid(board, currentCell, value)) return false;
        return true;
    }
    
    public SudokuBoard SolveBoard(SudokuBoard board)
    {
        SolveSudoku(board);
        return board;
    }

    private bool SolveSudoku(SudokuBoard sudokuBoard)
    {
        bool isEmpty = false;
        int row;
        int col = 0;
        if (FindEmptyCell(sudokuBoard, isEmpty, out row, ref col)) return true;
        if (ValidateCell(sudokuBoard, row, col)) return true;
        return false;
    }

    private bool ValidateCell(SudokuBoard sudokuBoard, int row, int col)
    {
        foreach (var num in _possibleValues)
        {
            if (Validate(sudokuBoard, row, col, num))
            {
                sudokuBoard.Cells[row, col].Value = num;

                if (SolveSudoku(sudokuBoard))
                {
                    return true;
                }
            }

            sudokuBoard.Cells[row, col].Value = 0;
        }

        return false;
    }

    private static bool FindEmptyCell(SudokuBoard sudokuBoard, bool isEmpty, out int row, ref int col)
    {
        for (row = 0; row < 9; row++)
        {
            for (col = 0; col < 9; col++)
            {
                var currentCell = sudokuBoard.Cells[row, col];
                if (currentCell.Value is 0)
                {
                    isEmpty = true;
                    break;
                }
            }

            if (isEmpty)
            {
                break;
            }
        }

        if (!isEmpty)
        {
            return true;
        }

        return false;
    }

    private bool IsRowValid(SudokuBoard board, SudokuCell cell, int value)
    {
        for (int i = 0; i < 9; i++)
        {
            if (board.Cells[cell.Placement.Row, i].Value == value)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsQuadrantValid(SudokuBoard board, SudokuCell cell, int value)
    {
        var quadrant = board.Quadrants[cell.Placement.Quadrant];
        if (quadrant.Any(x => x.Value == value))
        {
            return false;
        }

        return true;
    }

    private bool IsColValid(SudokuBoard board, SudokuCell cell, int value)
    {
        for (int i = 0; i < 9; i++)
        {
            if (board.Cells[i, cell.Placement.Column].Value == value)
            {
                return false;
            }
        }

        return true;
    }
}