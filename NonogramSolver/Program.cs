using System;
using System.Collections.Generic;
using System.Linq;

namespace NonogramSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var cluesRow = new List<List<int>>
            {
                //new List<int>() { 2 },
                //new List<int>() { 3 },
                //new List<int>() { 5,5},
                //new List<int>() { 6},
                //new List<int>() { 2},
                //new List<int>() { 3,6},
                //new List<int>() { 2,8},
                //new List<int>() { 10},
                //new List<int>() { 4,7},
                //new List<int>() { 4,3,4 },
                // new List<int>() { 4,5,3},
                //new List<int>() { 4,2,3,3},
                //new List<int>() { 3,2,5,2},
                //new List<int>() { 2,3,5,2},
                //new List<int>() { 1,4,6},

                // The duck
                new List<int>() { 3 },
                new List<int>() { 2,1 },
                new List<int>() { 3,2 },
                new List<int>() { 2,2 },
                new List<int>() { 6 },
                new List<int>() { 1,5 },
                new List<int>() { 6 },
                new List<int>() { 1 },
                new List<int>() { 2 },
            };

            var cluesCol = new List<List<int>>
            {
                //new List<int>() { 2,4},
                //new List<int>() { 2,4},
                //new List<int>() { 1,1,4,1},
                //new List<int>() { 2,4,2},
                //new List<int>() { 2,4,3},
                //new List<int>() { 2,4,4},
                //new List<int>() { 1,4,2},
                //new List<int>() { 3,2,3},
                //new List<int>() { 1,10},
                //new List<int>() { 1,2,10},
                //new List<int>() { 2,4,5},
                // new List<int>() { 2,5,3},
                //new List<int>() { 2,6,1},
                //new List<int>() { 3,7},
                //new List<int>() { 2,6},

                // The duck
                new List<int>() { 1,2 },
                new List<int>() { 3,1 },
                new List<int>() { 1,5 },
                new List<int>() { 7,1 },
                new List<int>() { 5 },
                new List<int>() { 3 },
                new List<int>() { 4 },
                new List<int>() { 3 },
                new List<int>() { 0 },
            };
            var size = 9;

            Matrix m = new Matrix(size, cluesRow, cluesCol);


            var jobList = fillJoblist(m, size);

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var jobs = jobList.Keys.ToList();
            while (jobs.Count > 0)
            {
                foreach (var job in jobList.Keys)
                {
                    if (job.state != RowState.completed)
                    {
                        var patternset = generateAllPatternSet(job, job.clues);
                        var overlappingIndex = retriveOverlapping(job, patternset);
                        fill(overlappingIndex, job);
                        setRowState(job);
                        //drawBoard(size, m);
                    }
                    else
                    {
                        jobs.Remove(job);
                    }
                }
            }
            drawBoard(size, m);
            Console.WriteLine("Complete");
            
        }

        private static int setRowState(Row job)
        {
            // Extract method
            var filledCells = job.cells.Select(x => x).Where(x => x.state == CellState.fill).ToList();
            var crossedCells = job.cells.Select(x => x).Where(x => x.state == CellState.cross).ToList();
            var score = job.clues.Sum() + (job.clues.Count - 1) + crossedCells.Count() - filledCells.Count();

            var total = filledCells.Count() + crossedCells.Count();

            if (total < job.cells.Count && total > 0)
            {
                job.state = RowState.started;
            }
            else if (total == job.cells.Count)
            {
                job.state = RowState.completed;
            }
            else
            {
                job.state = RowState.empty;
            }

            return score;
        }

        private static void drawBoard(int size, Matrix m)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    switch (m.Rows[i].cells[j].state)
                    {
                    
                        case CellState.fill:
                            Console.Write("|\u2588|");
                            break;
                        case CellState.cross:
                            Console.Write("|x|");
                            break;
                        case CellState.empty:
                            Console.Write("| |");
                            break;
                    }
                }
                Console.WriteLine("");
            }
            Console.WriteLine(" ");
        }

        private static Dictionary<Row, int> fillJoblist(Matrix m, int size)
        {
            var dic = new Dictionary<Row, int>();
            for (int i = 0; i < size; i++)
            {
                var colScore = size;
                var rowScore = size;
                if (m.Cols[i].clues.Sum() > 0)
                {
                    colScore = m.Cols[i].clues.Sum() + m.Cols[i].clues.Count - 1;
                }
                if (m.Rows[i].clues.Sum() > 0)
                {
                    rowScore = m.Rows[i].clues.Sum() + m.Rows[i].clues.Count - 1;
                }
                dic[m.Cols[i]] = colScore;
                dic[m.Rows[i]] = rowScore;
            }
            return dic;
        }

        private static List<List<int>> retriveOverlapping(Row row, List<List<int>> patternset)
        {
            var fillIndex = new List<int>();
            var crossIndex = new List<int>();
            if (patternset == null) return null;
            var initial = true;
            foreach (var pattern in patternset)
            {
                var temp = new Row();
                temp = testFill(pattern, row);
                if (initial)
                {
                    initial = false;
                    for (int j = 0; j < temp.cells.Count; j++)
                    {
                        if (temp.cells[j].state == CellState.fill)
                        {
                            fillIndex.Add(j);
                        }
                        else if (temp.cells[j].state == CellState.cross)
                        {
                            crossIndex.Add(j);
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < temp.cells.Count; j++)
                    {
                        if (fillIndex.Contains(j))
                        {
                            if (temp.cells[j].state != CellState.fill)
                            {
                                fillIndex.Remove(j);
                            }
                        }
                        else if (crossIndex.Contains(j))
                        {
                            if (temp.cells[j].state != CellState.cross)
                            {
                                crossIndex.Remove(j);
                            }
                        }

                    }
                }
            }
            return new List<List<int>>() { fillIndex, crossIndex };
        }

        private static List<List<int>> generateAllPatternSet(Row row, List<int> clues)
        {
            int size = row.cells.Count;
            int zeros = size - clues.Sum();
            var patternset = recursivePatternSet(zeros, size);
            if (patternset != null)
            {
                var removePattern = removePatternSet(row, clues, patternset);
                removePattern.ForEach(p =>
                {
                    if (patternset.Contains(p))
                        patternset.Remove(p);
                });
                if (patternset.Count == 0)
                {
                    patternset = null;
                }
            }
            return patternset;
        }

        private static List<List<int>> removePatternSet(Row row, List<int> clues, List<List<int>> patternset)
        {
            var removePattern = new List<List<int>>();
            foreach (var pattern in patternset)
            {
                // Test fill according to pattern
                var rowTemp = new Row();
                try
                {
                    rowTemp = testFill(pattern, row);
                }
                catch (Exception)
                {
                    removePattern.Add(pattern);
                    continue;
                }

                // validate solution
                var index = 0;
                for (int i = 0; i < row.cells.Count; i++)
                {
                    if (rowTemp.cells[i].state == CellState.fill)
                    {
                        // j ->
                        var currentClue = clues[index];
                        if (i + currentClue > row.cells.Count)
                        {
                            i = 10000;
                            removePattern.Add(pattern);
                            break;
                        }

                        for (int j = i; j < i + currentClue; j++)
                        {
                            if (rowTemp.cells[j].state != CellState.fill)
                            {
                                i = j = 10000;
                                removePattern.Add(pattern);
                                break;
                            }
                        }
                        i += currentClue - 1;
                        // check if it is a filled block after the clues
                        if (i + 1 < row.cells.Count)
                        {
                            if (rowTemp.cells[i + 1].state == CellState.fill)
                            {
                                removePattern.Add(pattern);
                                break;
                            }
                        }
                        if (index < clues.Count - 1)
                        {
                            index++;
                        }

                    }
                }
            }
            return removePattern;
        }

        private static Row testFill(List<int> pattern, Row row)
        {
            int size = row.cells.Count;
            var clone = row.Clone();
            for (int i = 0; i < size; i++)
            {
                if (!pattern.Contains(i))
                {
                    if (clone.cells[i].state == CellState.cross)
                    {
                        throw new Exception();
                    }
                    clone.cells[i].state = CellState.fill;
                }
                else
                {
                    if (clone.cells[i].state == CellState.fill)
                    {
                        throw new Exception();
                    }
                    clone.cells[i].state = CellState.cross;
                }
            }
            return clone;
        }

        private static void fill(List<List<int>> pattern, Row row)
        {
            if (pattern == null)
            {
                for (int i = 0; i < row.cells.Count; i++)
                {
                    row.cells[i].state = CellState.fill;
                }
                row.state = RowState.completed;
                return;
            }
            else
            {
                var fillPattern = pattern[0];
                var crossPattern = pattern[1];
                for (int i = 0; i < row.cells.Count; i++)
                {
                    if (fillPattern.Contains(i))
                    {
                        row.cells[i].state = CellState.fill;
                    }
                    if (crossPattern.Contains(i))
                    {
                        row.cells[i].state = CellState.cross;
                    }
                }
            }
        }

        private static List<List<int>> recursivePatternSet(int zeros, int size, List<int> before = null)
        {
            if (zeros == 0)
            {
                if (before == null) return null;
                var x = new List<List<int>>();
                x.Add(before);
                return x;
            }
            var patternset = new List<List<int>>();
            for (int i = size - 1; i >= 0; i--)
            {
                var p = (before == null ? new List<int>() : new List<int>(before));
                p.Add(i);
                patternset.AddRange(recursivePatternSet((zeros - 1), i, p));
            }
            return patternset;
        }

    }
}
