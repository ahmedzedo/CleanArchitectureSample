using CleanArchitecture.Application.Common.Abstracts.Account;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Common.Errors;
using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Common.Security;

namespace CleanArchitecture.Application.Users.Commands.RevokeRefreshToken
{
    #region Request

    [Authorize]
    public record RevokeRefreshTokenCommand(string RefreshToken) : BaseCommand<bool>
    {

    }
    #endregion

    #region Rquest Handler
    public sealed class RevokeRefreshTokenCommandHandler : BaseCommandHandler<RevokeRefreshTokenCommand, bool>
    {
        #region Dependencies
        private ITokenService TokenService { get; set; }
        #endregion

        #region Constructor
        public RevokeRefreshTokenCommandHandler(IServiceProvider serviceProvider,
                                                IApplicationDbContext dbContext,
                                                ITokenService tokenService)
           : base(serviceProvider, dbContext)
        {
            TokenService = tokenService;
        }
        #endregion

        #region Request Handle
        public async override Task<IResult<bool>> HandleRequest(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var result = await TokenService.RevokeRefreshToken(request.RefreshToken, UserId!);

            return result ? Result.Success(1) : Result.Failure(SecurityAccessErrors.NotAuthenticatedUser);
        }

        #endregion
    }
    #endregion


}
