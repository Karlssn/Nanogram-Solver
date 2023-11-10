using System.Collections.Generic;

namespace NonogramSolver.Data;

public class Line
{
    public List<Cell> Cells { get; set; }
    public LineState State { get; set; }
    public List<int> Clues { get; set; }

    public Line()
    {
        Cells = [];
        State = new LineState();
        Clues = [];
    }

    private Line(int size)
    {
        Cells = [];
        State = LineState.Empty;
        for (int i = 0; i < size; i++)
        {
            Cells.Add(new Cell());
        }
        Clues = [];
    }

    public Line Clone()
    {
        var clone = new Line(Cells.Count);
        for (int i = 0; i < Cells.Count; i++)
        {
            clone.Cells[i].State = Cells[i].State;
        }

        return clone;
    }
}
