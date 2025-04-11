using CleanArchitecture.Application.Common.Abstracts.Account;
using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Users.Commands.Dtos;
using CleanArchitecture.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Identity.Services
{
    public class UserService : IUserService
    {
        #region Dependencies
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUser CurrentUser;
        #endregion

        #region Constructor
        public UserService(UserManager<ApplicationUser> userManager,
                              ICurrentUser currentUser)
        {
            _userManager = userManager;
            CurrentUser = currentUser;
        }
        #endregion

        public async Task<bool> CheckPasswordAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            return user is not null && await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<(IResult<bool> Result, string UserId)> CreateUserAsync(string userName, string email, string password)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email,
            };

            var result = await _userManager.CreateAsync(user, password);

            return (result.ToApplicationResult(), user.Id);
        }

        public async Task<(IResult<bool> Result, string UserId)> CreateUserAsync(UserDto userDto, string password)
        {
            var user = new ApplicationUser
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                MiddleName = userDto.MiddleName,
                ThirdName = userDto.ThirdName,
                FamilyName = userDto.FamilyName,
            };

            var result = await _userManager.CreateAsync(user, password);

            return (result.ToApplicationResult(), user.Id);
        }

        public async Task<IResult<bool>> DeleteUserAsync(string userId)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);

            return user != null ? await DeleteUserAsync(user) : Result.Success(true);
        }
        private async Task<IResult<bool>> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }

        public async Task<UserDto?> GetCurrentUserAsync()
        {
            var user = await _userManager.FindByNameAsync(CurrentUser.Username);

            return user is not null
                ? new UserDto
                {
                    Id = Guid.Parse(user.Id),
                    Email = user.Email,
                    UserName = user.UserName,
                }
                : null;
        }

        public async Task<string?> GetUserAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            return user?.Id;
        }

        public async Task<UserDto?> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            return user is not null
                           ? new UserDto
                           {
                               Id = Guid.Parse(user.Id),
                               Email = user.Email,
                               UserName = user.UserName,
                               FirstName = user.FirstName,
                               MiddleName = user.MiddleName,
                               ThirdName = user.ThirdName,
                               FamilyName = user.FamilyName
                           }
                           : null;
        }

        public async Task<string?> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            return user?.UserName;
        }

        public async Task UpdateSecurityStampAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user is null)
            {
                return;
            }
            await _userManager.UpdateSecurityStampAsync(user);
        }
    }
}
