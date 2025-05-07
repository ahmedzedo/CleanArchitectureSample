using CleanArchitecture.Domain.Common.Entities;

namespace CleanArchitecture.Domain.Users
{
    public class RefreshToken : Entity, IAggregateRoot
    {
        public Guid Id { get; set; }
        public required string Token { get; set; }
        public required string UserId { get; set; }
        public string? DeviceId { get; set; }
        public DateTime ExpiresOnUtc { get; set; }
        public bool IsRevoked { get; set; }
        public User? User { get; set; }

    }
}
