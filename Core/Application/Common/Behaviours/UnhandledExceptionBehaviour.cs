using System.Diagnostics;
using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Common.Behaviours
{
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IRequestResponsePipeline<TRequest, TResponse>
          where TRequest : IAppRequest<TResponse>
    {
        #region Dependencies
        private readonly ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> _logger;
        #endregion

        #region Constructor

        public UnhandledExceptionBehaviour(ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;

        }
        #endregion

        #region Handel
        public async Task<IResult<TResponse>> Handle(TRequest request,
                                                       MyRequestHandlerDelegate<TResponse> next,
                                                       CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Unhandled Exception");
            var requestName = typeof(TRequest).Name;

            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: "CleanArchitecture Request: Unhandled Exception for Request {@Name} {@Request} {@Message}", requestName, request, ex.Message);

                return Result.Failure<TResponse>(Error.ThrowException(ex));
            }
        }
        #endregion
    }
}
