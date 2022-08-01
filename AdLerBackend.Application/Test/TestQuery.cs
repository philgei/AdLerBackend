using MediatR;

namespace AdLerBackend.Application.Test;

public record TestQuery : IRequest<string>;