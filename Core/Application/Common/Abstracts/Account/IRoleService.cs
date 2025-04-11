using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common.Abstracts.Account
{
    public interface IRoleService
    {
        Task<IList<string>> GetUserRolesAsync(string userId);
        Task<IList<string>> GetUserRolesPermissionsAsync(string userId);
        Task<bool> IsInRoleAsync(string userId, string role);
        Task<bool> HasUserPermissonAsync(string userId, string permission);
        Task<bool> AuthorizeAsync(string userId, string policyName);
    }
}
