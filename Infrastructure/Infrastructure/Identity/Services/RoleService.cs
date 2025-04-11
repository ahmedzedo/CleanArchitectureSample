using CleanArchitecture.Application.Common.Abstracts.Account;
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
    
    public class RoleService : IRoleService
    {

        #region Dependencies
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;
        #endregion

        #region Constructor
        public RoleService(UserManager<ApplicationUser> userManager,
                              IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
                              IAuthorizationService authorizationService,
                              RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
            _roleManager = roleManager;
        }
        #endregion

        public async Task<bool> AuthorizeAsync(string userId, string policyName)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);

            if (user is null)
            {
                return false;
            }

            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            if (principal is null)
            {
                return false;
            }
            var result = await _authorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;

        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            IList<string> roles = [];
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            if (user is not null)
            {
                roles = await _userManager.GetRolesAsync(user);
            }

            return roles;
        }

        public async Task<IList<string>> GetUserRolesPermissionsAsync(string userId)
        {
            var userRoles = await GetUserRolesAsync(userId);

            return await GetRolesPermissionsAsync(userRoles);
        }
        private async Task<IList<string>> GetRolesPermissionsAsync(IList<string> roles)
        {
            var permissions = new List<string>();

            foreach (var userRole in roles)
            {
                var rolePermissions = await _roleManager.GetRolePermissionsAsync(userRole);
                permissions.AddRange(rolePermissions);
            }

            return permissions;
        }
        public async Task<bool> HasUserPermissonAsync(string userId, string permission)
        {
            var userPermission = await GetUserRolesPermissionsAsync(userId);

            return userPermission.Any(p => p == permission);
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);

            return user != null && await _userManager.IsInRoleAsync(user, role);
        }
    }
}
