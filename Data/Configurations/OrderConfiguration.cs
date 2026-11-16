using KodaStore.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KodaStore.Api.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders", "dbo", table => table.HasCheckConstraint("CK_Orders_Status", "[Status] In ('Pending', 'Confirmed')"));

        builder.HasKey(o => o.OrderId);

        builder.Property(o => o.OrderId).ValueGeneratedOnAdd();

        builder.Property(o => o.OrderNumber).HasMaxLength(35).IsRequired();

        builder.Property(o => o.OrderDate).HasColumnType("datetimeoffset").IsRequired();

        builder.Property(o => o.Status).HasMaxLength(20).IsRequired();

        builder.HasIndex(o => o.OrderDate);
    }
}