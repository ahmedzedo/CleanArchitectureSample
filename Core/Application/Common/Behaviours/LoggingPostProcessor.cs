using CleanArchitecture.Application.Common.Messaging;
using MediatR;
using System.Diagnostics;

namespace CleanArchitecture.Application.Common.Behaviours
{
    public class LoggingPostProcessor<TRequest> : IRequestPostProcessor<TRequest>
        where TRequest : notnull, IRequest
    {
        public Task Handle(TRequest request, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Post processor");

            return Task.CompletedTask;
        }
    }
}
