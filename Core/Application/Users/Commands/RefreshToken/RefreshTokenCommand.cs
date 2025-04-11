using CleanArchitecture.Application.Common.Abstracts.Account;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Common.Errors;
using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Users.Commands.Dtos;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Users.Commands.RefreshToken
{

    #region Request

    public record RefreshTokenCommand(string RefreshToken) : BaseCommand<TokenResponse>
    {
    }
    #endregion

    #region Rquest Handler
    public sealed class RefreshTokenCommandHandler : BaseCommandHandler<RefreshTokenCommand, TokenResponse>
    {
        #region Dependencies
        private ITokenService TokenService { get; }
        #endregion

        #region Constructor
        public RefreshTokenCommandHandler(IServiceProvider serviceProvider,
                                          IApplicationDbContext dbContext,
                                          ITokenService tokenService)
           : base(serviceProvider, dbContext)
        {
            TokenService = tokenService;
        }
        #endregion

        #region Request Handle
        public override async Task<IResult<TokenResponse>> HandleRequest(RefreshTokenCommand request,
                                                                         CancellationToken cancellationToken)
        {
            return await TokenService.RefreshAccessToken(request.RefreshToken, cancellationToken);
        }

        #endregion
    }
    #endregion


}
