using FluentValidation;

namespace Microsoft.Extensions.DependencyInjection.Moodle.Commands;

public class MoodleLoginCommandValidator : AbstractValidator<MoodleLoginCommand>
{
    public MoodleLoginCommandValidator()
    {
        RuleFor(v => v.UserName)
            .NotEmpty().WithMessage("Username is required - PG")
            .MinimumLength(5).WithMessage("Min 5 carsacters - PG");
    }
}