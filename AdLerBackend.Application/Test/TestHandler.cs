using MediatR;

namespace AdLerBackend.Application.Test;

public class TestHandler : IRequestHandler<TestQuery, string>
{
    public Task<string> Handle(TestQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult("test :) ");
    }
}