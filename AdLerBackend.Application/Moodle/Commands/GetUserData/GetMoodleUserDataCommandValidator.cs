using FluentValidation;

namespace AdLerBackend.Application.Moodle.Commands.GetUserData;

public class GetMoodleUserDataCommandValidator : AbstractValidator<GetMoodleUserDataCommand>
{
    public GetMoodleUserDataCommandValidator()
    {
        RuleFor(v => v.UserName)
            .NotEmpty().WithMessage("Username is required");

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}