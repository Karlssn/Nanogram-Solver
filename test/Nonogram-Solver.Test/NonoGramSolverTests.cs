using NonogramSolver.Data;
using NUnit.Framework;
using System.Text.Json;

namespace NonogramSolver.Test;

[TestFixture]
public class NonogramSolverTests
{
    [Test]
    public void Solve_ExtraZeroColumn_ValidMatrix()
    {
        // Arrange
        var rowClues = new List<List<int>>
        {
            new() { 1 },
            new() { 0 },
            new() { 1 },
        };

        var columnClues = new List<List<int>>
        {
            new() { 1,0,1 },
            new() { 0 },
            new() { 0 },
        };
        var size = 3;
        Matrix matrix = new(size, rowClues, columnClues);

        var jsonSolution =
            """
[
    { "State":1},{ "State":2},{ "State":2},
    { "State":2},{ "State":2},{ "State":2},
    { "State":1},{ "State":2},{ "State":2}
]
""";

        List<Cell> solution = JsonSerializer.Deserialize<List<Cell>>(jsonSolution) ?? throw new ArgumentException();

        // Act
        Solver.Solve(matrix);
        foreach (var (item, i) in matrix.AllCells.Select((x, i) => (x.State, i)))
        {
            Console.Write(item);
            if (i % size == size - 1)
            {
                Console.WriteLine("");
            }
        }
        DrawBoard(size, matrix);

        // Assert
        Assert.That(solution.Select(x => (int)x.State), Is.EqualTo(matrix.AllCells.Select(x => (int)x.State)));
    }

    [Test]
    public void Solve_TheDuck_ValidMatrix()
    {
        // Arrange
        var rowClues = new List<List<int>>
        {
            new() { 3 },
            new() { 2, 1 },
            new() { 3, 2 },
            new() { 2, 2 },
            new() { 6 },
            new() { 1, 5 },
            new() { 6 },
            new() { 1 },
            new() { 2 },
        };

        var columnClues = new List<List<int>>
        {
            new() { 1,2 },
            new() { 3,1 },
            new() { 1,5 },
            new() { 7,1 },
            new() { 5 },
            new() { 3 },
            new() { 4 },
            new() { 3 },
            new() { 0 },
        };
        var size = 9;
        Matrix matrix = new(size, rowClues, columnClues);

        var jsonSolution =
            """
[
    { "State":2},{ "State":1},{ "State":1},{ "State":1},{ "State":2},{ "State":2},{ "State":2},{ "State":2},{ "State":2},
    { "State":1},{ "State":1},{ "State":2},{ "State":1},{ "State":2},{ "State":2},{ "State":2},{ "State":2},{ "State":2},
    { "State":2},{ "State":1},{ "State":1},{ "State":1},{ "State":2},{ "State":2},{ "State":1},{ "State":1},{ "State":2},
    { "State":2},{ "State":2},{ "State":1},{ "State":1},{ "State":2},{ "State":2},{ "State":1},{ "State":1},{ "State":2},
    { "State":2},{ "State":2},{ "State":1},{ "State":1},{ "State":1},{ "State":1},{ "State":1},{ "State":1},{ "State":2},
    { "State":1},{ "State":2},{ "State":1},{ "State":1},{ "State":1},{ "State":1},{ "State":1},{ "State":2},{ "State":2},
    { "State":1},{ "State":1},{ "State":1},{ "State":1},{ "State":1},{ "State":1},{ "State":2},{ "State":2},{ "State":2},
    { "State":2},{ "State":2},{ "State":2},{ "State":2},{ "State":1},{ "State":2},{ "State":2},{ "State":2},{ "State":2},
    { "State":2},{ "State":2},{ "State":2},{ "State":1},{ "State":1},{ "State":2},{ "State":2},{ "State":2},{ "State":2}
]
""";
        List<Cell> solution = JsonSerializer.Deserialize<List<Cell>>(jsonSolution) ?? throw new ArgumentException();

        // Act
        Solver.Solve(matrix);
        DrawBoard(size, matrix);

        // Assert
        Assert.That(solution.Select(x => (int)x.State), Is.EqualTo(matrix.AllCells.Select(x => (int)x.State)));
    }

    [Test]
    public void Solve_15by15_ValidMatrix()
    {
        // Arrange
        var rowClues = new List<List<int>>
        {
            new() { 2 },
            new() { 3 },
            new() { 5,5},
            new() { 6},
            new() { 2},
            new() { 3,6},
            new() { 2,8},
            new() { 10},
            new() { 4,7},
            new() { 4,3,4 },
            new() { 4,5,3},
            new() { 4,2,3,3},
            new() { 3,2,5,2},
            new() { 2,3,5,2},
            new() { 1,4,6},
        };

        var columnClues = new List<List<int>>
        {
            new() { 2,4},
            new() { 2,4},
            new() { 1,1,4,1},
            new() { 2,4,2},
            new() { 2,4,3},
            new() { 2,4,4},
            new() { 1,4,2},
            new() { 3,2,3},
            new() { 1,10},
            new() { 1,2,10},
            new() { 2,4,5},
            new() { 2,5,3},
            new() { 2,6,1},
            new() { 3,7},
            new() { 2,6},
        };
        var size = 15;

        Matrix matrix = new(size, rowClues, columnClues);

        var jsonSolution =
            """
[
    {"State":2},{"State":2},{"State":2},{"State":2},{"State":2},{"State":2},{"State":2},{"State":2},{"State":1},{"State":1},{"State":2},{"State":2},{"State":2},{"State":2},{"State":2},
    {"State":2},{"State":2},{"State":2},{"State":1},{"State":1},{"State":1},{"State":2},{"State":2},{"State":2},{"State":2},{"State":2},{"State":2},{"State":2},{"State":2},{"State":2},
    {"State":2},{"State":2},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},{"State":2},{"State":2},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},{"State":2},
    {"State":2},{"State":2},{"State":2},{"State":2},{"State":2},{"State":2},{"State":2},{"State":2},{"State":2},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},
    {"State":2},{"State":2},{"State":2},{"State":2},{"State":2},{"State":2},{"State":2},{"State":2},{"State":2},{"State":2},{"State":2},{"State":2},{"State":2},{"State":1},{"State":1},
    {"State":1},{"State":1},{"State":1},{"State":2},{"State":2},{"State":2},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},{"State":2},{"State":2},{"State":2},
    {"State":1},{"State":1},{"State":2},{"State":2},{"State":2},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},{"State":2},{"State":2},
    {"State":2},{"State":2},{"State":2},{"State":2},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},{"State":2},
    {"State":2},{"State":2},{"State":2},{"State":1},{"State":1},{"State":1},{"State":1},{"State":2},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},
    {"State":2},{"State":2},{"State":1},{"State":1},{"State":1},{"State":1},{"State":2},{"State":1},{"State":1},{"State":1},{"State":2},{"State":1},{"State":1},{"State":1},{"State":1},
    {"State":2},{"State":1},{"State":1},{"State":1},{"State":1},{"State":2},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},{"State":2},{"State":1},{"State":1},{"State":1},
    {"State":1},{"State":1},{"State":1},{"State":1},{"State":2},{"State":1},{"State":1},{"State":2},{"State":1},{"State":1},{"State":1},{"State":2},{"State":1},{"State":1},{"State":1},
    {"State":1},{"State":1},{"State":1},{"State":2},{"State":1},{"State":1},{"State":2},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},{"State":2},{"State":1},{"State":1},
    {"State":1},{"State":1},{"State":2},{"State":1},{"State":1},{"State":1},{"State":2},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},{"State":2},{"State":1},{"State":1},
    {"State":1},{"State":2},{"State":1},{"State":1},{"State":1},{"State":1},{"State":2},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},{"State":1},{"State":2},{"State":2}
]
""";

        List<Cell> solution = JsonSerializer.Deserialize<List<Cell>>(jsonSolution) ?? throw new ArgumentException();

        // Act
        Solver.Solve(matrix);
        DrawBoard(size, matrix);

        // Arrange
        Assert.That(solution.Select(x => (int)x.State), Is.EqualTo(matrix.AllCells.Select(x => (int)x.State)));
    }

    private static void DrawBoard(int size, Matrix m)
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                switch (m.Rows[i].Cells[j].State)
                {

                    case CellState.Fill:
                        Console.Write("|\u2588|");
                        break;
                    case CellState.Cross:
                        Console.Write("|x|");
                        break;
                    case CellState.Empty:
                        Console.Write("| |");
                        break;
                }
            }
            Console.WriteLine("");
        }
        Console.WriteLine(" ");
    }
}