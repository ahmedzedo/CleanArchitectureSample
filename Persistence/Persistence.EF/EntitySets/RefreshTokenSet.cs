using CleanArchitecture.Application.Users.IEntitySet;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Persistence.EF.EntitySets
{
    public class RefreshTokenSet : EntitySet<RefreshToken>, IRefreshTokenSet
    {
        public RefreshTokenSet(IDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<RefreshToken?> GetRefreshTokenWithUser(string token, CancellationToken cancellationToken = default)
        {
            EntityQuery = EntityQuery.Where(r => r.Token == token);

            return await this.JoinUser(cancellationToken);
        }
        public async Task<RefreshToken?> JoinUser(CancellationToken cancellationToken = default)
        {
            return await EntityQuery.Join(Context.Set<ApplicationUser>(),
                 token => token.UserId,
                 user => user.Id,
                 (token, user) => new
                 {
                     token,
                     user
                 }).Select(c => new RefreshToken()
                 {
                     Id = c.token.Id,
                     DeviceId = c.token.DeviceId,
                     UserId = c.token.UserId,
                     Token = c.token.Token,
                     ExpiresOnUtc = c.token.ExpiresOnUtc,
                     IsRevoked = c.token.IsRevoked,
                     User = c.user
                 }).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
