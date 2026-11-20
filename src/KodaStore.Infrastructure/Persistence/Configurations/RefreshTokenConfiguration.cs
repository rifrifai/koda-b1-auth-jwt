using System;
using KodaStore.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KodaStore.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
	public void Configure(EntityTypeBuilder<RefreshToken> builder)
	{
			builder.ToTable("RefreshTokens", "dbo");
			builder.HasKey(token => token.Id);
			builder.Property(token => token.Id)
				.ValueGeneratedNever();
			builder.Property(token => token.TokenHash)
				.HasMaxLength(64)
				.IsUnicode(false).IsRequired();
			builder.HasIndex(token => token.TokenHash)
				.IsUnique();
			builder.HasIndex(token => new { token.UserId, token.FamilyId });
			builder.HasIndex(token => token.FamilyId);

			builder.HasOne(token => token.User)
				.WithMany()
				.HasForeignKey(token => token.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Property(token => token.CreatedAtUtc)
				.HasColumnType("datetimeoffset");
			builder.Property(token => token.ExpiresAtUtc)
				.HasColumnType("datetimeoffset");
			builder.Property(token => token.UsedAtUtc)
				.HasColumnType("datetimeoffset");
			builder.Property(token => token.RevokedAtUtc)
				.HasColumnType("datetimeoffset");
	}
}
