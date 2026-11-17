using KodaStore.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KodaStore.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<AppUser>
{
	public void Configure(EntityTypeBuilder<AppUser> builder)
	{
		builder.Property(user => user.DisplayName)
                .HasMaxLength(100)
                .IsRequired();

		builder.Property(user => user.CreatedAtUtc)
			.IsRequired();
	}
}