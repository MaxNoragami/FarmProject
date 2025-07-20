using FarmProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmProject.Infrastructure.Configurations;

public class OrderRequestConfiguration : IEntityTypeConfiguration<OrderRequest>
{
    public void Configure(EntityTypeBuilder<OrderRequest> builder)
    {
        builder.HasKey(or => or.Id);

        builder.Property(or => or.OrderId)
            .IsRequired();

        builder.Property(or => or.OffspringType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(or => or.Amount)
            .IsRequired();

        builder.Property(or => or.SacrificedAmount)
            .IsRequired();

        builder.HasOne(or => or.Cage)
            .WithMany()
            .HasForeignKey("CageId")
            .IsRequired();

        builder.Property(or => or.OrderRequestStatus)
            .IsRequired()
            .HasConversion<string>();

        builder.HasOne<Order>()
            .WithMany(o => o.OrderRequests)
            .HasForeignKey(or => or.OrderId)
            .IsRequired();
    }
}
