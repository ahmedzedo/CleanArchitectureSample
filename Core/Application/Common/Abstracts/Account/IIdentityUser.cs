﻿using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Users.Commands.Dtos;
using CleanArchitecture.Domain.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common.Abstracts.Account
{
    public interface IUserService
    {
        Task<string?> GetUserNameAsync(string userId);
            Task<UserDto?> GetUserByIdAsync(string userId);
        Task<string?> GetUserAsync(string userName);
        Task<UserDto?> GetCurrentUserAsync();
        Task<(IResult<bool> Result, string UserId)> CreateUserAsync(string userName, string email, string password);
        Task<(IResult<bool> Result, string UserId)> CreateUserAsync(UserDto userDto, string password);
        Task<bool> CheckPasswordAsync(string userName, string password);
        Task UpdateSecurityStampAsync(string userName);
        Task<IResult<bool>> DeleteUserAsync(string userId);
    }
}
