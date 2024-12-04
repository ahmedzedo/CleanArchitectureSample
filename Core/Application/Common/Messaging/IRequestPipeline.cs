using MediatR;

namespace CleanArchitecture.Application.Common.Messaging
{
    #region Request Pipline
    public interface IRequestPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBaseRequest<TResponse>
    {

    }
    #endregion

    #region Request Response Pipline
    public delegate Task<IResult<TResponse>> MyRequestHandlerDelegate<TResponse>();
    public interface IRequestResponsePipeline<TRequest, TResponse>
        where TRequest : IAppRequest<TResponse>
    {
        Task<IResult<TResponse>> Handle(TRequest request,
                                        MyRequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
    }
    #endregion

    #region Request PreProcessor
    public interface IRequestPreProcessor<in TRequest> where TRequest : notnull
    {
        Task Process(TRequest request, CancellationToken cancellationToken);
    }
    #endregion

    #region Request Post Processor
    /// <summary>
    /// Defines a request post-processor for a request
    /// </summary>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    public interface IRequestPostProcessor<in TRequest, TResponse> where TRequest : notnull
    {
        /// <summary>
        /// Process method executes after the Handle method on your handler
        /// </summary>
        /// <param name="request">Request instance</param>
        /// <param name="response">Response instance</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>An awaitable task</returns>
        Task Process(TRequest request, IResult<TResponse> response, CancellationToken cancellationToken);
    }
    #endregion


}
