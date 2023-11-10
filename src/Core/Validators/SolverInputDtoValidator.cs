using System.Linq;
using FluentValidation;
using NonogramSolver.Core.Services;

namespace NonogramSolver.Core.Validators;

public class SolverInputValidator : AbstractValidator<SolverInputDto>
{
    public SolverInputValidator()
    {
        RuleFor(input => input.Size)
            .GreaterThan(0)
            .WithMessage("The size of the puzzle must be greater than 0");
        RuleFor(input => input.ColClues.Count)
            .Equal(i => i.Size)
            .WithMessage("The number of column clues must be equal to the size of the puzzle");
        RuleFor(input => input.RowClues.Count)
            .Equal(i => i.Size)
            .WithMessage("The number of row clues must be equal to the size of the puzzle");
        RuleForEach(input => input.ColClues)
            .Must((input, i) => i.Sum() <= input.Size)
            .WithMessage("The sum of column clues must be less than or equal to the size of the puzzle");
        RuleForEach(input => input.RowClues)
            .Must((input, i) => i.Sum() <= input.Size)
            .WithMessage("The sum of row clues must be less than or equal to the size of the puzzle");
    }
}