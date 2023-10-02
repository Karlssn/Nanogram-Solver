using NonogramSolver.Data;
using System.Collections.Generic;
using System.Linq;

namespace NonogramSolver;

public static class Solver
{
    public static void Solve(int size, Matrix matrix)
    {
        var jobList = FillJoblist(matrix, size);

        var jobs = jobList.Keys.ToList();
        while (jobs.Count > 0)
        {
            foreach (var job in jobList.Keys)
            {
                if (job.State != RowState.Completed)
                {
                    var patternset = GenerateAllPatternSet(job, job.Clues);
                    var overlappingIndex = RetriveOverlapping(job, patternset);
                    Fill(overlappingIndex, job);
                    SetRowState(job);
                }
                else
                {
                    jobs.Remove(job);
                }
            }
        }
    }

    private static int SetRowState(Row job)
    {
        // Extract method
        var filledCells = job.Cells.Select(x => x).Where(x => x.State == CellState.Fill).ToList();
        var crossedCells = job.Cells.Select(x => x).Where(x => x.State == CellState.Cross).ToList();
        var score = job.Clues.Sum() + (job.Clues.Count - 1) + crossedCells.Count() - filledCells.Count();

        var total = filledCells.Count() + crossedCells.Count();

        if (total < job.Cells.Count && total > 0)
        {
            job.State = RowState.Started;
        }
        else if (total == job.Cells.Count)
        {
            job.State = RowState.Completed;
        }
        else
        {
            job.State = RowState.Empty;
        }

        return score;
    }

    private static Dictionary<Row, int> FillJoblist(Matrix m, int size)
    {
        var dic = new Dictionary<Row, int>();
        for (int i = 0; i < size; i++)
        {
            var colScore = size;
            var rowScore = size;
            if (m.Cols[i].Clues.Sum() > 0)
            {
                colScore = m.Cols[i].Clues.Sum() + m.Cols[i].Clues.Count - 1;
            }
            if (m.Rows[i].Clues.Sum() > 0)
            {
                rowScore = m.Rows[i].Clues.Sum() + m.Rows[i].Clues.Count - 1;
            }
            dic[m.Cols[i]] = colScore;
            dic[m.Rows[i]] = rowScore;
        }

        return dic;
    }

    private static List<List<int>> RetriveOverlapping(Row row, List<List<int>> patternset)
    {
        var fillIndex = new List<int>();
        var crossIndex = new List<int>();
        if (patternset == null) return null;

        var initial = true;
        foreach (var pattern in patternset)
        {
            var temp = new Row();
            temp = TestFill(pattern, row);
            if (initial)
            {
                initial = false;
                for (int j = 0; j < temp.Cells.Count; j++)
                {
                    if (temp.Cells[j].State == CellState.Fill)
                    {
                        fillIndex.Add(j);
                    }
                    else if (temp.Cells[j].State == CellState.Cross)
                    {
                        crossIndex.Add(j);
                    }
                }
            }
            else
            {
                for (int j = 0; j < temp.Cells.Count; j++)
                {
                    if (fillIndex.Contains(j))
                    {
                        if (temp.Cells[j].State != CellState.Fill)
                        {
                            fillIndex.Remove(j);
                        }
                    }
                    else if (crossIndex.Contains(j))
                    {
                        if (temp.Cells[j].State != CellState.Cross)
                        {
                            crossIndex.Remove(j);
                        }
                    }

                }
            }
        }

        return new List<List<int>>() { fillIndex, crossIndex };
    }

    private static List<List<int>> GenerateAllPatternSet(Row row, List<int> clues)
    {
        int size = row.Cells.Count;
        int zeros = size - clues.Sum();
        var patternset = RecursivePatternSet(zeros, size);
        if (patternset != null)
        {
            var removePattern = RemovePatternSet(row, clues, patternset);
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

    private static List<List<int>> RemovePatternSet(Row row, List<int> clues, List<List<int>> patternset)
    {
        var removePattern = new List<List<int>>();
        foreach (var pattern in patternset)
        {
            var rowTemp = new Row();
            try
            {
                rowTemp = TestFill(pattern, row);
            }
            catch (NonogramException)
            {
                removePattern.Add(pattern);
                continue;
            }

            // validate solution
            var index = 0;
            for (int i = 0; i < row.Cells.Count; i++)
            {
                if (rowTemp.Cells[i].State == CellState.Fill)
                {
                    // j ->
                    var currentClue = clues[index];
                    if (i + currentClue > row.Cells.Count)
                    {
                        i = 10000;
                        removePattern.Add(pattern);
                        break;
                    }

                    for (int j = i; j < i + currentClue; j++)
                    {
                        if (rowTemp.Cells[j].State != CellState.Fill)
                        {
                            i = j = 10000;
                            removePattern.Add(pattern);
                            break;
                        }
                    }
                    i += currentClue - 1;
                    // check if it is a filled block after the clues
                    if (i + 1 < row.Cells.Count)
                    {
                        if (rowTemp.Cells[i + 1].State == CellState.Fill)
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

    private static Row TestFill(List<int> pattern, Row row)
    {
        int size = row.Cells.Count;
        var clone = row.Clone();
        for (int i = 0; i < size; i++)
        {
            if (!pattern.Contains(i))
            {
                if (clone.Cells[i].State == CellState.Cross)
                {
                    throw new NonogramException("Failed to fill");
                }
                clone.Cells[i].State = CellState.Fill;
            }
            else
            {
                if (clone.Cells[i].State == CellState.Fill)
                {
                    throw new NonogramException("Failed to fill");
                }
                clone.Cells[i].State = CellState.Cross;
            }
        }

        return clone;
    }

    private static void Fill(List<List<int>> pattern, Row row)
    {
        if (pattern == null)
        {
            for (int i = 0; i < row.Cells.Count; i++)
            {
                row.Cells[i].State = CellState.Fill;
            }
            row.State = RowState.Completed;

            return;
        }
        else
        {
            var fillPattern = pattern[0];
            var crossPattern = pattern[1];
            for (int i = 0; i < row.Cells.Count; i++)
            {
                if (fillPattern.Contains(i))
                {
                    row.Cells[i].State = CellState.Fill;
                }
                if (crossPattern.Contains(i))
                {
                    row.Cells[i].State = CellState.Cross;
                }
            }
        }
    }

    private static List<List<int>> RecursivePatternSet(int zeros, int size, List<int> before = null)
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
            patternset.AddRange(RecursivePatternSet((zeros - 1), i, p));
        }

        return patternset;
    }
}