namespace CleanArchitecture.Application.Common.Messaging
{
    #region Request Pipline
    public delegate Task<TResponse> MyRequestHandlerDelegate<TResponse>();
    public interface IRequestPipeline<TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request,
                               MyRequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
    }
    #endregion

    #region Request Response Pipline
    public delegate Task<IResult<TResponse>> MyRequestResponseHandlerDelegate<TResponse>();
    public interface IRequestResponsePipeline<TRequest, TResponse>
    {
        Task<IResult<TResponse>> Handle(TRequest request,
                                          MyRequestResponseHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
    }
    #endregion

    #region Request PreProcessor
    public interface IRequestPreProcessor<in TRequest> where TRequest : notnull
    {
        Task Handle(TRequest request, CancellationToken cancellationToken);

    }
    #endregion

    #region Request Post Processor
    public interface IRequestPostProcessor<in TRequest> where TRequest : notnull
    {
        Task Handle(TRequest request, CancellationToken cancellationToken);

    }
    #endregion


}
