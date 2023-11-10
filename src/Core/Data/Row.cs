using System.Collections.Generic;

namespace NonogramSolver.Data;

public class Row
{
    public List<Cell> Cells { get; set; }
    public RowState State { get; set; }
    public List<int> Clues { get; set; }

    public Row()
    {
        Cells = new List<Cell>();
        State = new RowState();
    }

    public Row(int size)
    {
        Cells = new List<Cell>();
        State = RowState.Empty;
        for (int i = 0; i < size; i++)
        {
            Cells.Add(new Cell());
        }
    }

    public Row Clone()
    {
        var clone = new Row(Cells.Count);
        for (int i = 0; i < Cells.Count; i++)
        {
            clone.Cells[i].State = Cells[i].State;
        }

        return clone;
    }
}
