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

        builder.Property(or => or.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(or => or.Amount)
            .IsRequired();

        builder.Property(or => or.CageId)
            .IsRequired();
    }
}
