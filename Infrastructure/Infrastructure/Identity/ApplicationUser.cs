using CleanArchitecture.Domain.Common.Entities;
using CleanArchitecture.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser//, IApplicationUser
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
            SecurityStamp = Guid.NewGuid().ToString();
        }
        public ApplicationUser(string userName)
        {
            Id = Guid.NewGuid().ToString();
            SecurityStamp = Guid.NewGuid().ToString();
        }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? ThirdName { get; set; }
        public string? FamilyName { get; set; }
        public bool? Otpactivate { get; set; }
        public bool Gender { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "Anonymous";
        public DateTime? LastUpdatedOn { get; set; }
        public string? LastUpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOnUtc { get; set; }

        public static implicit operator User(ApplicationUser applicationUser)
        {
            return new User
            {
                Id = applicationUser.Id,
                FirstName = applicationUser.FirstName,
                MiddleName = applicationUser.MiddleName,
                ThirdName = applicationUser.ThirdName,
                FamilyName = applicationUser.FamilyName,
                AccessFailedCount = applicationUser.AccessFailedCount,
                ConcurrencyStamp = applicationUser.ConcurrencyStamp,
                CreatedBy = applicationUser.CreatedBy,
                CreatedOn = applicationUser.CreatedOn,
                DeletedOnUtc = applicationUser.DeletedOnUtc,
                Email = applicationUser.Email,
                EmailConfirmed = applicationUser.EmailConfirmed,
                Gender = applicationUser.Gender,
                IsDeleted = applicationUser.IsDeleted,
                LastUpdatedBy = applicationUser.LastUpdatedBy,
                LastUpdatedOn = applicationUser.LastUpdatedOn,
                LockoutEnabled = applicationUser.LockoutEnabled,
                LockoutEnd = applicationUser.LockoutEnd,
                NormalizedEmail = applicationUser.NormalizedEmail,
                NormalizedUserName = applicationUser.NormalizedUserName,
                PasswordHash = applicationUser.PasswordHash,
                PhoneNumber = applicationUser.PhoneNumber,
                PhoneNumberConfirmed = applicationUser.PhoneNumberConfirmed,
                SecurityStamp = applicationUser.SecurityStamp,
                TwoFactorEnabled = applicationUser.TwoFactorEnabled,
                UserName = applicationUser.UserName
            };
        }
        public static implicit operator ApplicationUser(User user)
        {
            return new ApplicationUser
            {
                Id = user.Id,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                ThirdName = user.ThirdName,
                FamilyName = user.FamilyName,
                AccessFailedCount = user.AccessFailedCount,
                ConcurrencyStamp = user.ConcurrencyStamp,
                CreatedBy = user.CreatedBy,
                CreatedOn = user.CreatedOn,
                DeletedOnUtc = user.DeletedOnUtc,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Gender = user.Gender,
                IsDeleted = user.IsDeleted,
                LastUpdatedBy = user.LastUpdatedBy,
                LastUpdatedOn = user.LastUpdatedOn,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd = user.LockoutEnd,
                NormalizedEmail = user.NormalizedEmail,
                NormalizedUserName = user.NormalizedUserName,
                PasswordHash = user.PasswordHash,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                SecurityStamp = user.SecurityStamp,
                TwoFactorEnabled = user.TwoFactorEnabled,
                UserName = user.UserName
            };
        }
    }
}
