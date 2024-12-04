using MediatR;

namespace CleanArchitecture.Application.Common.Messaging
{
    public interface IBaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
         where TRequest : IBaseRequest<TResponse>
    {
    }

    public interface IAppRequestHandler<TRequest, TResponse> : IBaseRequestHandler<TRequest, IResult<TResponse>>
        where TRequest : IAppRequest<TResponse>
    {
    }
    public interface ICommandHandler<TRequest, TResponse> : IAppRequestHandler<TRequest, TResponse>
       where TRequest : IBaseCommand<TResponse>
    {
    }
    public interface IQueryHandler<TRequest, TResponse> : IAppRequestHandler<TRequest, TResponse>
     where TRequest : IBaseQuery<TResponse>
    {
    }
}
