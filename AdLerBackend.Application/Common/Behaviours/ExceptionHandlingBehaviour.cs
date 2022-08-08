using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Exceptions.LMSAdapter;
using MediatR;
using MediatR.Pipeline;

namespace AdLerBackend.Application.Common.Behaviours;

public class
    ExceptionHandlingBehaviour<TRequest, TResponse, TException> : IRequestExceptionHandler<TRequest, TResponse,
        TException> where TRequest : IRequest<TResponse> where TException : LmsException
{
    public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state,
        CancellationToken cancellationToken)
    {
        if (exception is LmsException ex)
            switch (ex.LmsErrorCode)
            {
                case "invalidtoken":
                    throw new InvalidTokenException();
            }

        throw exception;
    }
}