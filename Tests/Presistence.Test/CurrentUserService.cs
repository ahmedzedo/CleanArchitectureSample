using CleanArchitecture.Application.Common.Abstracts.Account;

namespace Presistence.Test
{
    public class CurrentUser : ICurrentUser
    {
        public string UserId => Guid.NewGuid().ToString();

        public string Username => "Admin";
    }
}
