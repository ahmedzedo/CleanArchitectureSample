using MediatR;

namespace CleanArchitecture.Application.Common.Messaging
{
    public enum RequestType
    {
        BaseRequest,
        AppRequest,
        Command,
        Query,
        PagedListQuery

    }
    public interface IBaseRequest<out TResponse> : IRequest<TResponse>
    {
        public string? UserName { get; set; }
        public string? UserId { get; set; }
        public string RequestName => GetType().Name;
        public RequestType RequestType { get; protected init; }
    }
    public interface IAppRequest<TResponse> : IBaseRequest<IResult<TResponse>>
    {
    }
    public interface IBaseCommand<TResponse> : IAppRequest<TResponse>
    {
    }
    public interface IBaseQuery<TResponse> : IAppRequest<TResponse>
    {
    }
    public interface IPagedListQuery<TResponse> : IAppRequest<TResponse>
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PagePerPages { get; set; }

        public string? OrderByPropertyName { get; set; }

        public string? SortDirection { get; set; }
    }
}
