using System.Reflection;
using CleanArchitecture.Application.Common.Abstracts.Account;
using CleanArchitecture.Application.Common.Errors;
using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Domain.Constants;

namespace CleanArchitecture.Application.Common.Behaviours
{
    public class AuthorizationBehaviour<TRequest, TResponse> : IRequestResponsePipeline<TRequest, TResponse>
        where TRequest : IAppRequest<TResponse>
    {
        #region Dependencies
        public IUserService UserService { get; set; }
        public IRoleService RoleService { get; set; }
        #endregion

        #region Constructor
        public AuthorizationBehaviour(IUserService userService, IRoleService roleService)
        {
            UserService = userService;
            RoleService = roleService;
        }
        #endregion

        #region Handle

        public async Task<IResult<TResponse>> Handle(TRequest request, MyRequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

            if (authorizeAttributes.Any())
            {
                // Must be authenticated user
                var user = await UserService.GetCurrentUserAsync();

                if (user is null)
                {
                    return Result.Failure<TResponse>(SecurityAccessErrors.NotAuthenticatedUser);
                }

                // Role-based authorization
                var authorizeAttributesWithRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles));

                if (authorizeAttributesWithRoles.Any())
                {
                    var authorized = false;

                    foreach (var roles in authorizeAttributesWithRoles.Select(a => a.Roles.Split(',')))
                    {
                        foreach (var role in roles)
                        {
                            var isInRole = await RoleService.IsInRoleAsync(user.Id.ToString(), role.Trim());

                            if (isInRole)
                            {
                                authorized = true;
                                break;
                            }
                        }
                    }

                    // Must be a member of at least one role in roles
                    if (!authorized)
                    {
                        return Result.Failure<TResponse>(SecurityAccessErrors.ForbiddenAccess);
                    }
                }

                // Policy-based authorization
                var authorizeAttributesWithPolicies = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy));

                if (authorizeAttributesWithPolicies.Any())
                {
                    foreach (var policy in authorizeAttributesWithPolicies.Select(a => a.Policy))
                    {
                        var IsPermission = policy.Contains(Permissions.CLAIM_TYPE);

                        if (IsPermission)
                        {
                            var IsUserHasPermission = await RoleService.HasUserPermissonAsync(user.Id.ToString(), policy);

                            if (!IsUserHasPermission)
                            {
                                return Result.Failure<TResponse>(SecurityAccessErrors.ForbiddenAccess);
                            }
                        }
                        else
                        {
                            var authorized = await RoleService.AuthorizeAsync(user.Id.ToString(), policy);

                            if (!authorized)
                            {
                                return Result.Failure<TResponse>(SecurityAccessErrors.ForbiddenAccess);
                            }
                        }
                    }
                }
            }

            // User is authorized / authorization not required
            return await next();
        }
        #endregion
    }
}
