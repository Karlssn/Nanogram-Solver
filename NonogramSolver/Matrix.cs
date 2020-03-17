using System;
using System.Collections.Generic;
using System.Text;

namespace NonogramSolver
{
    class Matrix
    {
        public List<Cell> AllCells { get; set; }
        public List<Row> Rows { get; set; }
        public List<Row> Cols { get; set; }

        public Matrix(int n,List<List<int>> rowClues, List<List<int>> colClues)
        {
            AllCells = new List<Cell>();
            Rows = new List<Row>();
            Cols = new List<Row>();
            var arrRow = new Cell[n];
            var arrCol = new Cell[n];
            for (int i = 0; i < n*n; i++)
            {
                var c = new Cell();
                AllCells.Add(c);
            }

            //[1,2,3]

            //Add the same cells to rows
            for (int i = 0; i < n*n; i+=n)
            {
                var tempRow = new Row();
                var r = new Cell[n];
                for (int j = 0; j < n; j++) {
                    r[j] = AllCells[j+i];
                }
                tempRow.cells.AddRange(r);
                Rows.Add(tempRow);
            }
            
            for (int i = 0; i < n; i++)
            {
                var tempCol = new Row();
                for (int j = 0; j < n*n; j+=n)
                {
                    var c = AllCells[i + j];
                    tempCol.cells.Add(c);
                }
                Cols.Add(tempCol);
            }

            // add the clues 
            for (int i = 0; i < n; i++)
            {
                Rows[i].clues = rowClues[i];
                Cols[i].clues = colClues[i];
            }
        }

    }
}
