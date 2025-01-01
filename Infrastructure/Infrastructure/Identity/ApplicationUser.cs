using CleanArchitecture.Domain.Common.Entities;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser, IAuditableEntity, ISoftDeletable
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? ThirdName { get; set; }
        public string? FamilyName { get; set; }
        public bool? Otpactivate { get; protected set; }
        public bool Gender { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "Anonymous";
        public DateTime? LastUpdatedOn { get; set; }
        public string? LastUpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOnUtc { get; set; }
    }
}
