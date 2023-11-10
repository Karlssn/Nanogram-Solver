namespace NonogramSolver.Data;

public enum CellState
{
    Empty = 0,
    Fill = 1,
    Cross = 2,
}

public enum LineState
{
    Empty,
    Started,
    Completed
}
