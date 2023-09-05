using System;
using System.Collections.Generic;
using System.Linq;
using NonogramSolver.Data;
using NonogramSolver;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var cluesRow = new List<List<int>>
{
    new List<int>() { 2 },
    new List<int>() { 3 },
    new List<int>() { 5,5},
    new List<int>() { 6},
    new List<int>() { 2},
    new List<int>() { 3,6},
    new List<int>() { 2,8},
    new List<int>() { 10},
    new List<int>() { 4,7},
    new List<int>() { 4,3,4 },
    new List<int>() { 4,5,3},
    new List<int>() { 4,2,3,3},
    new List<int>() { 3,2,5,2},
    new List<int>() { 2,3,5,2},
    new List<int>() { 1,4,6},

    // The duck
    //new List<int>() { 3 },
    //new List<int>() { 2,1 },
    //new List<int>() { 3,2 },
    //new List<int>() { 2,2 },
    //new List<int>() { 6 },
    //new List<int>() { 1,5 },
    //new List<int>() { 6 },
    //new List<int>() { 1 },
    //new List<int>() { 2 },
};

var cluesCol = new List<List<int>>
{
    new List<int>() { 2,4},
    new List<int>() { 2,4},
    new List<int>() { 1,1,4,1},
    new List<int>() { 2,4,2},
    new List<int>() { 2,4,3},
    new List<int>() { 2,4,4},
    new List<int>() { 1,4,2},
    new List<int>() { 3,2,3},
    new List<int>() { 1,10},
    new List<int>() { 1,2,10},
    new List<int>() { 2,4,5},
    new List<int>() { 2,5,3},
    new List<int>() { 2,6,1},
    new List<int>() { 3,7},
    new List<int>() { 2,6},

    // The duck
    //new List<int>() { 1,2 },
    //new List<int>() { 3,1 },
    //new List<int>() { 1,5 },
    //new List<int>() { 7,1 },
    //new List<int>() { 5 },
    //new List<int>() { 3 },
    //new List<int>() { 4 },
    //new List<int>() { 3 },
    //new List<int>() { 0 },
};
var size = 15;

Matrix matrix = new Matrix(size, cluesRow, cluesCol);

Solver.Solve(size, matrix);

DrawBoard(size, matrix);
Console.WriteLine("Complete");

void DrawBoard(int size, Matrix m)
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
