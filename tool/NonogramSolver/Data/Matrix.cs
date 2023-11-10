using System.Collections.Generic;
using System.Linq;

namespace NonogramSolver.Data;

public class Matrix
{
    public List<Cell> AllCells { get; private set; }
    public List<Line> Rows { get; private set; }
    public List<Line> Columns { get; private set; }

    public Matrix(int size, List<List<int>> rowClues, List<List<int>> columnClues)
    {
        Rows = [];
        Columns = [];

        AllCells = Enumerable
            .Range(0, size * size)
            .Select(_ => new Cell()).ToList();

        //Add the same cells to rows

        for (int i = 0; i < size * size; i += size)
        {
            var row = new Line();
            for (int j = 0; j < size; j++)
            {
                var cell = AllCells[i + j];
                row.Cells.Add(cell);
            }

            Rows.Add(row);
        }

        for (int i = 0; i < size; i++)
        {
            var column = new Line();
            for (int j = 0; j < size * size; j += size)
            {
                var cell = AllCells[i + j];
                column.Cells.Add(cell);
            }

            Columns.Add(column);
        }

        // add the Clues 
        for (int i = 0; i < size; i++)
        {
            Rows[i].Clues = rowClues[i];
            Columns[i].Clues = columnClues[i];
        }
    }
}