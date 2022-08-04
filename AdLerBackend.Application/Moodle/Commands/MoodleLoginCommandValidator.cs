using FluentValidation;

namespace AdLerBackend.Application.Moodle.Commands;

public class MoodleLoginCommandValidator : AbstractValidator<MoodleLoginCommand>
{
    public MoodleLoginCommandValidator()
    {
        RuleFor(v => v.UserName)
            .NotEmpty().WithMessage("Username is required");

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}