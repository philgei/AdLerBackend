using AdLerBackend.Application.Common.Responses;
using AdLerBackend.Application.Moodle.Commands.GetLearningWorldDSL;
using MediatR;

namespace AdLerBackend.Application.Moodle.Handlers;

public class GetLearningWorldDslHandler : IRequestHandler<GetLearningWorldDslCommand, LearningWorldDtoResponse>
{
    public Task<LearningWorldDtoResponse> Handle(GetLearningWorldDslCommand request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException("The handler for this command has not been implemented yet.");
    }
}