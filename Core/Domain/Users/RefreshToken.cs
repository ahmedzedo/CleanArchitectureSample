using CleanArchitecture.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
