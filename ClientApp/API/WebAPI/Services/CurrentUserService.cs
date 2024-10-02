using CleanArchitecture.Application.Common.Abstracts.Account;
using System.Security.Claims;

namespace CleanArchitecture.WebAPI.Services
{
    public class CurrentUser : ICurrentUser
    {
        #region Dependencies
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructor
        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Properties
        public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anonymous";
        public string Username => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name) ?? "Anonymous";
        #endregion

    }
}
