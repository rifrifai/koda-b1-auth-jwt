using KodaStore.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KodaStore.Api.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product", "dbo", table =>
            {
                table.HasCheckConstraint("CK_Product_Price", "[Price] >= 0");

                table.HasCheckConstraint("CK_Product_Stock", "[Stock] >= 0");
            });

            builder.HasKey(p => p.ProductCode);

            builder.Property(p => p.ProductCode)
                .HasMaxLength(30)
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(p => p.Name)
                .HasMaxLength(120)
                .IsRequired();

            builder.Property(p => p.Price)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(p => p.Stock)
                .IsRequired();

            builder.Property(p => p.Category)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(p => p.CategoryId).IsRequired(false);
            builder.Property(p => p.RowVersion).IsRowVersion();

            builder.HasOne(p => p.CategoryEntity)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.CategoryId);

            builder.HasIndex(p => new
            {
                p.Category,
                p.Price,
                p.Name
            }).HasDatabaseName("IX_Product_Category_Price_Name");
    }
}