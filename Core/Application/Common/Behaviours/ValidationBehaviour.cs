using CleanArchitecture.Application.Common.Errors;
using CleanArchitecture.Application.Common.Extensions;
using CleanArchitecture.Application.Common.Messaging;
using FluentValidation;

namespace CleanArchitecture.Application.Common.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IRequestResponsePipeline<TRequest, TResponse>
          where TRequest : IBaseRequest<IResult<TResponse>>
    {
        #region Dependencies
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        #endregion

        #region Constructors
        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        #endregion

        #region Handle
        public async Task<IResult<TResponse>> Handle(TRequest request,
                                                         MyRequestResponseHandlerDelegate<TResponse> next,
                                                       CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                {
                    return Result.Failure<TResponse>(ValidationErrors.FluentValidationErrors(failures.ToDictionary()),
                                                       $"at {request.GetType().AssemblyQualifiedName}");
                }
            }
            return await next();
        }

        #endregion
    }
}
