using Microsoft.AspNetCore.Identity;

namespace SchoolTripApi.Infrastructure.Security.Entities;

internal sealed class Account : IdentityUser<Guid>
{
    public bool UnlockMessageSent { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}