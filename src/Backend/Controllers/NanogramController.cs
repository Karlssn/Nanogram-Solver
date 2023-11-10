using Microsoft.AspNetCore.Mvc;
using NonogramSolver.Core.Services;
using NonogramSolver.Data;

namespace Backend;

[ApiController]
public class NanogramController(ISolverService solverService)
{
    [HttpPost("solve")]
    public async Task<ActionResult<List<List<CellEnumDto>>>> SolveAsync(SolverInputDto dto)
    {
        return await solverService.Solve(dto);
    }
}
