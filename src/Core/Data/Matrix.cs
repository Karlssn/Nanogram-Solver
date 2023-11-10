using System.Collections.Generic;

namespace NonogramSolver.Data;

public class Matrix
{
    public List<Cell> AllCells { get; set; }
    public List<Row> Rows { get; set; }
    public List<Row> Cols { get; set; }

    public Matrix(int size, List<List<int>> rowClues, List<List<int>> colClues)
    {
        AllCells = new List<Cell>();
        Rows = new List<Row>();
        Cols = new List<Row>();
        var arrRow = new Cell[size];
        var arrCol = new Cell[size];
        for (int i = 0; i < size * size; i++)
        {
            var c = new Cell();
            AllCells.Add(c);
        }

        //Add the same cells to rows
        for (int i = 0; i < size * size; i += size)
        {
            var tempRow = new Row();
            var r = new Cell[size];
            for (int j = 0; j < size; j++)
            {
                r[j] = AllCells[j + i];
            }

            tempRow.Cells.AddRange(r);
            Rows.Add(tempRow);
        }

        for (int i = 0; i < size; i++)
        {
            var tempCol = new Row();
            for (int j = 0; j < size * size; j += size)
            {
                var c = AllCells[i + j];
                tempCol.Cells.Add(c);
            }

            Cols.Add(tempCol);
        }

        // add the Clues 
        for (int i = 0; i < size; i++)
        {
            Rows[i].Clues = rowClues[i];
            Cols[i].Clues = colClues[i];
        }
    }
}