using KodaStore.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KodaStore.Api.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItem", "dbo", table =>
            {
                table.HasCheckConstraint("CK_OrderItem_Quantity", "[Quantity] > 0");
                table.HasCheckConstraint("CK_OrderItem_UnitPrice", "[UnitPrice] >= 0");
            });

        builder.HasKey(i => i.OrderItemId);

        builder.Property(i => i.OrderItemId).ValueGeneratedOnAdd().IsRequired();

        builder.Property(i => i.ProductCode).HasMaxLength(30).IsRequired();

        builder.Property(i => i.Quantity).IsRequired();

        builder.Property(i => i.UnitPrice).HasPrecision(18, 2).IsRequired();

        builder.Property(i => i.Subtotal).HasComputedColumnSql("[Quantity] * [UnitPrice]", stored: true);

        builder.HasOne(i => i.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(i => i.ProductCode)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(i => new { i.OrderId, i.ProductCode }).IsUnique();
        builder.HasIndex(i => i.ProductCode);
    }
}