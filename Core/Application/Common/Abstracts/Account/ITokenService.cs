using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Users.Commands.Dtos;
using CleanArchitecture.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common.Abstracts.Account
{
    public interface ITokenService
    {
        Task<IResult<TokenResponse>> GenerateTokenAsync(string userName);
        Task<RefreshToken> GenerateRefreshToken(string userId, string? deviceId = null);
        Task<bool> ValidateRefreshToken(string token);
        Task<bool> RevokeRefreshToken(string token, string userId);
        Task<IResult<TokenResponse>> RefreshAccessToken(string token, CancellationToken cancellationToken = default);
    }
}
