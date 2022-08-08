using MediatR;

namespace AdLerBackend.Application.Common;

public record CommandWithToken<TResponse> : IRequest<TResponse>
{
    public string WebServiceToken { get; set; }
}