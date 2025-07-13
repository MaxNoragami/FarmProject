using FarmProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmProject.Infrastructure.Configurations;

public class SacrificationConfiguration : IEntityTypeConfiguration<Sacrification>
{
    public void Configure(EntityTypeBuilder<Sacrification> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property<int>("OrderId")
            .IsRequired();

        builder.Property(s => s.BreedingRabbitId)
            .IsRequired();

        builder.Property(s => s.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(s => s.Amount)
            .IsRequired();

        builder.Property(s => s.BirthDate)
            .IsRequired();
    }
}
