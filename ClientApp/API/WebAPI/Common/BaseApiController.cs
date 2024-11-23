using CleanArchitecture.Application.Common.Messaging;
using Common.DependencyInjection.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebAPI.Common
{
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        #region Dependencies

        private IMediator? _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetInstance<IMediator>();
        #endregion

        #region Constructors
        public BaseApiController()
        {

        }
        #endregion

        #region Properties

        #endregion

        #region Method
        [NonAction]
        public ObjectResult Result<T>(IResult<T> response)
        {
            return response.IsSuccess
                ? Ok(response)
                : Problem(response);
        }

        public ObjectResult Problem<T>(IResult<T> response)
        {
            var result = new ObjectResult(new ExtendedProblemdetails()
            {
                Title = "An error Occured",
                Status = response.StatusCode,
                Detail = response.Error?.Message,
                Instance = $"{response.Source} Path:{HttpContext.Request.Path}/{HttpContext.Request.Method} Host:{HttpContext.Request.Host} Protocol:{HttpContext.Request.Protocol}",
                SubErrors = response.Error?.SubErrors
            });

            return result;
        }
        public class ExtendedProblemdetails : ProblemDetails
        {
            public Dictionary<string, string>? SubErrors { get; set; } = [];
        }


        #endregion
    }
}
