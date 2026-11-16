using KodaStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KodaStore.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
	{
		builder.ToTable("Category", "dbo");

		builder.HasKey(c => c.CategoryId);

		builder.Property(c => c.CategoryId).ValueGeneratedOnAdd();

		builder.Property(c => c.Name).HasMaxLength(30).IsRequired();
		
		builder.HasIndex(c => c.Name).IsUnique();
	}
}