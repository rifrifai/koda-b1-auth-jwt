using Microsoft.AspNetCore.Identity;

namespace KodaStore.Infrastructure.Identity;

public sealed class AppUser : IdentityUser<Guid>
{
	public string DisplayName { get; set; } = "";
	public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}