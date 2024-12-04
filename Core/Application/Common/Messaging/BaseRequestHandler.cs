using AutoMapper;
using CleanArchitecture.Application.Common.Abstracts.Account;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using Common.DependencyInjection.Extensions;

namespace CleanArchitecture.Application.Common.Messaging
{
    #region Class BaseRequestHandler
    public abstract class BaseRequestHandler<TRequest, TResponse> : IBaseRequestHandler<TRequest, TResponse>
     where TRequest : IBaseRequest<TResponse>
    {
        #region Dependencies
        protected IServiceProvider ServiceProvider { get; }
        private ICurrentUser? CurrentUserService => ServiceProvider.GetInstance<ICurrentUser>();
        #endregion

        #region Properties
        public string? UserId => CurrentUserService?.UserId;
        public string Username => CurrentUserService?.Username ?? "Anonymous";
        #endregion

        #region Constructor
        protected BaseRequestHandler(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        #endregion

        #region Handel
        public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            SetUserInfo(request);
            return await HandleRequest(request, cancellationToken);
        }
        public abstract Task<TResponse> HandleRequest(TRequest request, CancellationToken cancellationToken);
        #endregion

        #region Methods
        protected void SetUserInfo(TRequest request)
        {
            request.UserName = Username != "Anonymous" ? Username : request.UserName;
            request.UserId = UserId;
        }
        #endregion
    }
    #endregion

    #region Class AppRequestHandler
    public abstract class AppRequestHandler<TRequest, TResponse> : BaseRequestHandler<TRequest, IResult<TResponse>>, IAppRequestHandler<TRequest, TResponse>
           where TRequest : IAppRequest<TResponse>
    {
        #region Dependencies
        protected IApplicationDbContext DbContext { get; }
        protected IMapper Mapper => ServiceProvider.GetInstance<IMapper>();
        #endregion

        #region Constructor
        protected AppRequestHandler(IServiceProvider serviceProvider,
                                    IApplicationDbContext dbContext)
           : base(serviceProvider)
        {
            DbContext = dbContext;
        }
        #endregion

        #region Handel
        public override async Task<IResult<TResponse>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            SetUserInfo(request);

            return await GetResponse(request, cancellationToken);
        }
        public override abstract Task<IResult<TResponse>> HandleRequest(TRequest request, CancellationToken cancellationToken);

        #endregion

        #region Helper Methods

        private async Task<IResult<TResponse>> GetResponse(TRequest request, CancellationToken cancellationToken)
        {
            await ExecutePreProcessorBehaviours(request, cancellationToken);

            var response = await ExcuteRequestPiplineBehaviours(request, cancellationToken);

            await ExecutePostProcessorBehaviours(request, response, cancellationToken);

            return response;
        }
        private async Task ExecutePreProcessorBehaviours(TRequest request, CancellationToken cancellationToken)
        {
            var preProcessors = ServiceProvider.GetInstances<IRequestPreProcessor<TRequest>>();

            if (preProcessors.Any())
            {
                foreach (var preprocessor in preProcessors)
                {
                    await preprocessor.Process(request, cancellationToken);
                }
            }
        }

        private async Task<IResult<TResponse>> ExcuteRequestPiplineBehaviours(TRequest request, CancellationToken cancellationToken)
        {
            IResult<TResponse> response;
            var requestPipelines = ServiceProvider.GetInstances<IRequestResponsePipeline<TRequest, TResponse>>();

            if (requestPipelines.Any())
            {
                Task<IResult<TResponse>> Handler() => HandleRequest(request, cancellationToken);
                response = await requestPipelines
                        .Reverse()
                        .Aggregate((MyRequestHandlerDelegate<TResponse>)Handler, (next, pipeline) => () => pipeline.Handle(request, next, cancellationToken))();
            }
            else
            {
                response = await HandleRequest(request, cancellationToken);
            }

            return response;
        }

        private async Task ExecutePostProcessorBehaviours(TRequest request, IResult<TResponse> result, CancellationToken cancellationToken)
        {
            var postProcessors = ServiceProvider.GetInstances<IRequestPostProcessor<TRequest, TResponse>>();

            if (postProcessors.Any())
            {
                foreach (var postProcessor in postProcessors)
                {
                    await postProcessor.Process(request, result, cancellationToken);
                }
            }
        }

        #endregion
    }
    #endregion

    #region Class BaseCommandHandler
    public abstract class BaseCommandHandler<TRequest, TResponse> : AppRequestHandler<TRequest, TResponse>, ICommandHandler<TRequest, TResponse>
    where TRequest : IBaseCommand<TResponse>
    {
        protected BaseCommandHandler(IServiceProvider serviceProvider, IApplicationDbContext dbContext)
          : base(serviceProvider, dbContext)
        {

        }
    }

    #endregion

    #region Class BaseQueryHandler
    public abstract class BaseQueryHandler<TRequest, TResponse> : AppRequestHandler<TRequest, TResponse>, IQueryHandler<TRequest, TResponse>
    where TRequest : IBaseQuery<TResponse>
    {
        protected BaseQueryHandler(IServiceProvider serviceProvider, IApplicationDbContext dbContext)
          : base(serviceProvider, dbContext)
        {

        }
    }
    #endregion


}
