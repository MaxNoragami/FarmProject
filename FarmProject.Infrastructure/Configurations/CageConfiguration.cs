using FarmProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmProject.Infrastructure.Configurations;

public class CageConfiguration : IEntityTypeConfiguration<Cage>
{
    public void Configure(EntityTypeBuilder<Cage> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(c => c.OffspringType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(c => c.OffspringCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.HasOne(c => c.BreedingRabbit)
            .WithMany()
            .HasForeignKey("BreedingRabbitId")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
