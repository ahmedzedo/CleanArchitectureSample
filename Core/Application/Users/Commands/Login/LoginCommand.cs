using CleanArchitecture.Application.Common.Abstracts.Account;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Users.Commands.Dtos;

namespace CleanArchitecture.Application.Users.Commands.Login
{
    #region Request
    public record LoginCommand(string Password) : BaseCommand<TokenResponse>
    {
        public new required string UserName { get; init; }
    }

    #endregion

    #region Request Handler
    public class LoginCommandHandler : BaseCommandHandler<LoginCommand, TokenResponse>
    {
        #region Dependencies
        private ITokenService TokenService { get; }

        #endregion

        #region Constructor
        public LoginCommandHandler(IServiceProvider serviceProvider,
                                   IApplicationDbContext dbContext,
                                   ITokenService tokenService)
           : base(serviceProvider, dbContext)
        {
            TokenService = tokenService;
        }
        #endregion

        #region Request Handle
        public override async Task<IResult<TokenResponse>> HandleRequest(LoginCommand request,
                                                                           CancellationToken cancellationToken)
        {
            return await TokenService.GenerateTokenAsync(request.UserName!);
        }


        #endregion
    }
    #endregion
}
