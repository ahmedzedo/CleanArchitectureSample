using CleanArchitecture.Application.Common.Errors;
using CleanArchitecture.Application.Common.Messaging;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Identity
{
    public static class IdentityResultExtensions
    {
        public static Result<bool> ToApplicationResult(this IdentityResult result)
        {

            var errors = result.Errors
                 .Select(e => new { e.Code, e.Description })
                 .ToDictionary(e => e.Code, e => e.Description);

            return result.Succeeded
                ? Result.Success(true)
                : Result.Failure(SecurityAccessErrors.CreationUserFailed(errors));
        }
    }
}