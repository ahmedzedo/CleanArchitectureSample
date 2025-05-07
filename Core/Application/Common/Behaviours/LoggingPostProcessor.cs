using System.Diagnostics;
using CleanArchitecture.Application.Common.Messaging;

namespace CleanArchitecture.Application.Common.Behaviours
{
    public class LoggingPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
        where TRequest : notnull, IAppRequest<TResponse>
    {
        public Task Process(TRequest request,
                           IResult<TResponse> response,
                           CancellationToken cancellationToken)
        {
            Debug.WriteLine("Post processor");

            return Task.CompletedTask;
        }
    }
}
