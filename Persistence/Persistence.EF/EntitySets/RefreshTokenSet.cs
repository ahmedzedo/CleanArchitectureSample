using CleanArchitecture.Application.Users.IEntitySet;
using CleanArchitecture.Domain.Common.Entities;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Persistence.EF.EntitySets
{
    public class RefreshTokenSet : EntitySet<RefreshToken>, IRefreshTokenSet
    {
        public RefreshTokenSet(IDbContext dbContext) : base(dbContext)
        {
        }

    }
}
