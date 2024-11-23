using CleanArchitecture.Application.Common.Abstracts.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistence.Test
{
    public class CurrentUser : ICurrentUser
    {
        public string UserId => Guid.NewGuid().ToString();

        public string Username => "Admin";
    }
}
