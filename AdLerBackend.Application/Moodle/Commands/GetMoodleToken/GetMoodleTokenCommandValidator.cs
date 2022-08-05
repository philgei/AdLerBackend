using FluentValidation;

namespace AdLerBackend.Application.Moodle.Commands.GetMoodleToken;

public class GetMoodleTokenCommandValidator : AbstractValidator<GetMoodleTokenCommand>
{
    public GetMoodleTokenCommandValidator()
    {
        RuleFor(v => v.UserName)
            .NotEmpty().WithMessage("Username is required");

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}