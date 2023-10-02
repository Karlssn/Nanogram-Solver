namespace NonogramSolver.Data;

public class Cell
{
    public CellState State { get; set; }

    public Cell()
    {
        State = CellState.Empty;
    }
}