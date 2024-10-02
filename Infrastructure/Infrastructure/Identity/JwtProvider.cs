using CleanArchitecture.Application.Users.Commands.Dtos;
using CleanArchitecture.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArchitecture.Infrastructure.Identity
{
    public class JwtProvider
    {
        #region Fields
        private const int EXPIRATION_MINUTES = 600;
        #endregion

        #region Dependencies
        private JwtOptions Options { get; }
        private UserManager<ApplicationUser> UserManager { get; }
        private RoleManager<IdentityRole> RoleManager { get; }
        #endregion

        #region Constructor
        public JwtProvider(IOptions<JwtOptions> options,
                          UserManager<ApplicationUser> userManager,
                          RoleManager<IdentityRole> roleManager)
        {
            Options = options.Value;
            UserManager = userManager;
            RoleManager = roleManager;
        }
        #endregion

        #region Methods
        public async Task<TokenResponse> GenerateAsync(ApplicationUser user)
        {
            var notBefor = DateTime.UtcNow;
            var claims = await CreateClaimsAsync(user, notBefor);
            SigningCredentials signingCredentials = CreateSigningCredentials();
            var expiration = DateTime.UtcNow.AddMinutes(EXPIRATION_MINUTES);
            var token = CreateToken(claims.ToArray(), notBefor, signingCredentials, expiration);

            return new TokenResponse(token, expiration);
        }

        private string CreateToken(Claim[] claims, DateTime notBefore, SigningCredentials signingCredentials, DateTime expiration)
        {
            var token = new JwtSecurityToken(
                Options.Issuer,
                Options.Audience,
                claims,
                notBefore,
                expiration,
                signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private SigningCredentials CreateSigningCredentials() => new(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Options.Key!)),
                                                                        SecurityAlgorithms.HmacSha256);

        private async Task<List<Claim>> CreateClaimsAsync(ApplicationUser user, DateTime notBefore)
        {
            var userRoles = await UserManager.GetRolesAsync(user);
            var permissions = await GetUserRolesPermissions(userRoles);
            var claims = new List<Claim> {
                //new Claim(JwtRegisteredClaimNames.Sub, Options.Subject!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, notBefore.ToString()),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.Email, user.Email!)
           };
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            claims.AddRange(permissions.Select(permission => new Claim(Permissions.CLAIM_TYPE, permission)));

            return claims;
        }

        private async Task<IList<string>> GetUserRolesPermissions(IList<string> userRoles)
        {
            var permissions = new List<string>();

            foreach (var userRole in userRoles)
            {
                var rolePermissions = await RoleManager.GetRolePermissionsAsync(userRole);
                permissions.AddRange(rolePermissions);
            }

            return permissions;
        }
        #endregion
    }
}
