using FarmProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmProject.Infrastructure.Configurations;

public class SacrificationConfiguration : IEntityTypeConfiguration<Sacrification>
{
    public void Configure(EntityTypeBuilder<Sacrification> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.OrderRequestId)
            .IsRequired(false);

        builder.Property(s => s.SacrificationReason)
            .IsRequired()
            .HasConversion<string>();

        builder.HasOne(or => or.Cage)
            .WithMany()
            .HasForeignKey("CageId")
            .IsRequired();

        builder.Property(s => s.OffspringType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(s => s.Amount)
            .IsRequired();

        builder.Property(s => s.BirthDate)
            .IsRequired();
    }
}
