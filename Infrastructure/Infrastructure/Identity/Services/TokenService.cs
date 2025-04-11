using CleanArchitecture.Application.Common.Abstracts.Account;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Common.Errors;
using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Users.Commands.Dtos;
using CleanArchitecture.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Identity.Services
{
    public class TokenService : ITokenService
    {
        #region Dependencies
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtProvider _jwtProvider;
        private readonly IApplicationDbContext _dbContext;
        #endregion

        #region Constructor
        public TokenService(UserManager<ApplicationUser> userManager,
                              JwtProvider jwtProvider,
                              IApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _jwtProvider = jwtProvider;
            _dbContext = dbContext;
        }
        #endregion

        #region Methods
        public async Task<RefreshToken> GenerateRefreshToken(string userId, string? deviceId = null)
        {
            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)), // Strong token
                ExpiresOnUtc = DateTime.UtcNow.AddDays(7), // Refresh token lifespan
                UserId = userId,
                DeviceId = deviceId,
                IsRevoked = false
            };

            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();

            return refreshToken;
        }

        public async Task<IResult<TokenResponse>> GenerateTokenAsync(string userName)
        {
            var appUser = await _userManager.FindByNameAsync(userName);

            if (appUser is null)
            {
                return Result.Failure<TokenResponse>(SecurityAccessErrors.NotAuthenticatedUser);
            }
            (var token, var expireation) = await _jwtProvider.GenerateAsync(appUser);
            var refreshToken = await GenerateRefreshToken(appUser.Id);

            return string.IsNullOrEmpty(token) || refreshToken is null
                ? Result.Failure<TokenResponse>(SecurityAccessErrors.NotAuthenticatedUser)
                : Result.Success(new TokenResponse(token, refreshToken.Token, expireation));
        }

        public async Task<IResult<TokenResponse>> RefreshAccessToken(string token, CancellationToken cancellationToken = default)
        {
            var refreshToken = await _dbContext.RefreshTokens.GetRefreshTokenWithUser(token,
                                                                                     cancellationToken);
            if (refreshToken == null || refreshToken.ExpiresOnUtc < DateTime.UtcNow || refreshToken.IsRevoked)
            {
                return Result.Failure<TokenResponse>(SecurityAccessErrors.NotAuthenticatedUser);
            }
            //var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == refreshToken.UserId.ToString(), cancellationToken: cancellationToken);

            if (refreshToken.User == null)
            {
                return Result.Failure<TokenResponse>(SecurityAccessErrors.NotAuthenticatedUser);
            }

            (token, var expireation) = await _jwtProvider.GenerateAsync(refreshToken.User);
            var newRefreshToken = await GenerateRefreshToken(refreshToken.User.Id);
            await RevokeRefreshToken(refreshToken, cancellationToken);

            return Result.Success(new TokenResponse(token, newRefreshToken.Token, expireation));
        }

        private async Task RevokeRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            refreshToken.IsRevoked = true;
            refreshToken.ExpiresOnUtc = DateTime.UtcNow.AddMinutes(-1);
            _dbContext.RefreshTokens.Update(refreshToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RevokeRefreshToken(string token, string userId)
        {
            var refreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
            var result = false;
           
            if (refreshToken != null && refreshToken.UserId.ToString() == userId)
            {
                refreshToken.IsRevoked = true;
                refreshToken.ExpiresOnUtc = DateTime.UtcNow.AddMinutes(-1);
                _dbContext.RefreshTokens.Update(refreshToken);
                result = (await _dbContext.SaveChangesAsync()) > 0;
            }

            return result;
        }

        public async Task<bool> ValidateRefreshToken(string token)
        {
            var refreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);

            return refreshToken != null
                   && !refreshToken.IsRevoked
                   && refreshToken.ExpiresOnUtc > DateTime.UtcNow;
        }
        #endregion
    }
}
