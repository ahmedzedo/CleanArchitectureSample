using CleanArchitecture.Application.Users.Commands.CreateUser;
using CleanArchitecture.Application.Users.Commands.Login;
using CleanArchitecture.Application.Users.Commands.RefreshToken;
using CleanArchitecture.Application.Users.Commands.RevokeRefreshToken;
using CleanArchitecture.WebAPI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebAPI.Controllers
{
    public class UserController : BaseApiController
    {
        #region Actions
        [HttpPost("user-login")]
        public async Task<IActionResult> UserLogin([FromBody] LoginCommand loginCommand)
        {
            return Result(await Mediator.Send(loginCommand));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand refreshTokenCommand)
        {
            return Result(await Mediator.Send(refreshTokenCommand));
        }
        [Authorize]
        [HttpPost("revoke-refreshtoken")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] RevokeRefreshTokenCommand revokeRefreshTokenCommand)
        {
            return Result(await Mediator.Send(revokeRefreshTokenCommand));
        }

        [HttpPost("Create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand loginCommand)
        {
            return Result(await Mediator.Send(loginCommand));
        }
        #endregion
    }
}
