using System;
using DotNetFiveApiDemo.Core.Common.Interfaces;

namespace DotNetFiveApiDemo.Core.Security.Entities
{
    public class RefreshToken<TUser> : IAggregateRoot where TUser : class, IAggregateRoot
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public int UserId { get; init; }

        public string Token { get; init; }

        public string TokenFamily { get; init; }

        public DateTime ExpiresAt { get; init; }

        public bool IsRevoked { get; set; }

        public TUser User { get; init; }

        public bool IsActive => !IsRevoked && DateTime.UtcNow < ExpiresAt;
        public bool IsExpired => DateTime.UtcNow > ExpiresAt;
    }
}