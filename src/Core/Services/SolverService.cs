using FluentValidation;
using NonogramSolver.Core.Validators;
using NonogramSolver.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NonogramSolver.Core.Services;

public interface ISolverService
{
    Task<List<List<CellEnumDto>>> Solve(SolverInputDto input);
}

public class SolverService : ISolverService
{
    public async Task<List<List<CellEnumDto>>> Solve(SolverInputDto input)
    {
        await new SolverInputValidator()
            .ValidateAndThrowAsync(input);

        var matrix = new Matrix(input.Size, input.RowClues, input.ColClues);
        Solver.Solve(input.Size, matrix);
        return matrix.Cols
            .Select(col => col.Cells
                .Select(x => x.State == CellState.Fill ? CellEnumDto.Filled : CellEnumDto.Cross)
                .ToList())
            .ToList();
    }
}

public record SolverInputDto(int Size, List<List<int>> ColClues, List<List<int>> RowClues);

public enum CellEnumDto
{
    Cross,
    Filled
}