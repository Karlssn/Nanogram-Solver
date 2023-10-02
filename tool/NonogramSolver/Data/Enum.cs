namespace NonogramSolver.Data;

public enum CellState
{
    Empty = 0,
    Fill = 1,
    Cross = 2,
}

public enum RowState
{
    Empty,
    Started,
    Completed
}
