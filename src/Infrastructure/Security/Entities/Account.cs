using Microsoft.AspNetCore.Identity;

namespace SchoolTripApi.Infrastructure.Security.Entities;

public class Account : IdentityUser
{
    public bool UnlockMessageSent { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}