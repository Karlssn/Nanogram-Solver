using NonogramSolver.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NonogramSolver.Core;

public static class Solver
{
    public static void Solve(Matrix matrix)
    {
        var running = true;
        while (running)
        {
            foreach (var line in matrix.Columns.Concat(matrix.Rows))
            {
                var patternSet = GenerateAllPatternSet(line);
                var (fill, cross) = GetIntersectedPatternSet(line, patternSet)
                    ?? SetFillOrCross(line.Cells.Count, line.Clues.Sum());

                UpdateLine(line, fill, cross);
            }

            running = matrix.Columns
                .Concat(matrix.Rows)
                .Any(x => x.State != LineState.Completed);
        }
    }

    private static List<List<int>>? GenerateAllPatternSet(Line line)
    {
        int size = line.Cells.Count;
        int zeros = size - line.Clues.Sum();
        var patternSet = RecursivePatternSet(zeros, size);
        if (patternSet != null)
        {
            RemoveInvalidPatternSet(line, patternSet);

            if (patternSet.Count == 0)
            {
                patternSet = null;
            }
        }

        return patternSet;
    }

    private static List<List<int>>? RecursivePatternSet(int zeros, int size, List<int>? previous = null)
    {
        if (zeros == 0)
        {
            return previous == null ? null : ([previous]);
        }

        var patternSet = new List<List<int>>();
        for (int i = size - 1; i >= 0; i--)
        {
            var current = previous == null ? [] : new List<int>(previous);
            current.Add(i);
            patternSet.AddRange(RecursivePatternSet(zeros - 1, i, current) ?? []);
        }

        return patternSet;
    }

    private static void RemoveInvalidPatternSet(Line line, List<List<int>> patternSet)
    {
        var removePattern = new List<List<int>>();
        foreach (var pattern in patternSet)
        {
            try
            {
                var testLine = TestFillThrows(pattern, line);
                ValidatePatternThrow(line, testLine);
            }
            catch (NonogramException)
            {
                removePattern.Add(pattern);
            }
        }

        removePattern.ForEach(p => patternSet.Remove(p));
    }

    private static Line TestFillThrows(List<int> pattern, Line line)
    {
        int size = line.Cells.Count;
        var clone = line.Clone();
        foreach (var (cell, i) in clone.Cells.Select((c, i) => (c, i)))
        {
            if (!pattern.Contains(i))
            {
                if (cell.State == CellState.Cross)
                {
                    throw new NonogramException("Failed to set fill");
                }
                cell.State = CellState.Fill;
            }
            else
            {
                if (cell.State == CellState.Fill)
                {
                    throw new NonogramException("Failed to set cross");
                }
                cell.State = CellState.Cross;
            }
        }

        return clone;
    }

    private static void ValidatePatternThrow(Line line, Line testLine)
    {
        var clues = line.Clues;
        var clueIndex = 0;
        for (int i = 0; i < line.Cells.Count; i++)
        {
            if (testLine.Cells[i].State == CellState.Fill)
            {
                var currentClue = clues[clueIndex];

                ValidateClueNotOutOfRangeThrows(line.Cells.Count, i, currentClue);

                ValidateAllCellsInClueAreFilledThrows(testLine, i, currentClue);

                i += currentClue - 1;

                ValidateCellAfterClueIsNotFillThrows(testLine, line.Cells.Count, i);

                if (clueIndex < clues.Count - 1)
                {
                    clueIndex++;
                }
            }
        }
    }

    private static void ValidateCellAfterClueIsNotFillThrows(Line filledLine, int lineCount, int index)
    {
        if (index + 1 < lineCount)
        {
            if (filledLine.Cells[index + 1].State == CellState.Fill)
            {
                throw new NonogramException("Cell after last clue can not be fill");
            }
        }
    }

    private static void ValidateAllCellsInClueAreFilledThrows(Line filledLine, int startIndex, int sizeOfClue)
    {
        for (int i = startIndex; i < startIndex + sizeOfClue; i++)
        {
            if (filledLine.Cells[i].State != CellState.Fill)
            {
                throw new NonogramException("All clues must be filled");
            }
        }
    }

    private static void ValidateClueNotOutOfRangeThrows(int lineCount, int index, int clueSize)
    {
        if (index + clueSize > lineCount)
        {
            throw new NonogramException("Clues must be in range");
        }
    }

    private static (List<int> FillIndexes, List<int> CrossIndexes)? GetIntersectedPatternSet(Line line, List<List<int>>? patternSet)
    {
        if (patternSet == null)
        {
            return null;
        }

        List<int>? fillIndex = null;
        List<int>? crossIndex = null;

        foreach (var pattern in patternSet)
        {
            var testLine = TestFillThrows(pattern, line);
            if (fillIndex is null || crossIndex is null)
            {
                fillIndex = GetIndicesFromState(testLine, CellState.Fill).ToList();
                crossIndex = GetIndicesFromState(testLine, CellState.Cross).ToList();
            }
            else
            {
                fillIndex = fillIndex.Intersect(GetIndicesFromState(testLine, CellState.Fill)).ToList();
                crossIndex = crossIndex.Intersect(GetIndicesFromState(testLine, CellState.Cross)).ToList();
            }
        }
        if (fillIndex is null || crossIndex is null)
        {
            throw new NonogramException("Failed to intersect pattern set");
        }

        return (fillIndex, crossIndex);
    }

    private static IEnumerable<int> GetIndicesFromState(Line testLine, CellState state)
    {
        for (int i = 0; i < testLine.Cells.Count; i++)
        {
            if (testLine.Cells[i].State == state)
            {
                yield return i;
            }
        }
    }

    private static (List<int> FillIndexes, List<int> CrossIndexes) SetFillOrCross(int size, int clueSum)
    {
        var allValues = Enumerable.Range(0, size).ToList();

        return clueSum == 0
            ? (new List<int>(), allValues)
            : (allValues, new List<int>());
    }

    private static void UpdateLine(Line line, List<int> fill, List<int> cross)
    {
        fill.ForEach(i =>
            line.Cells[i].State = CellState.Fill
        );
        cross.ForEach(i =>
            line.Cells[i].State = CellState.Cross
        );

        SetLineState(line);
    }

    private static void SetLineState(Line line)
    {
        var numberOfFilledCells = line.Cells
            .Select(x => x)
            .Where(x => x.State == CellState.Fill)
            .Count();

        var numberOfCrossedCells = line.Cells
            .Select(x => x)
            .Where(x => x.State == CellState.Cross)
            .Count();

        var total = numberOfFilledCells + numberOfCrossedCells;

        if (total < line.Cells.Count && total > 0)
        {
            line.State = LineState.Started;
        }
        else if (total == line.Cells.Count)
        {
            line.State = LineState.Completed;
        }
        else
        {
            line.State = LineState.Empty;
        }
    }
}