using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmProject.Infrastructure.Configurations;

public class BreedingRabbitConfiguration : IEntityTypeConfiguration<BreedingRabbit>
{
    public void Configure(EntityTypeBuilder<BreedingRabbit> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(52);

        builder.Property(r => r.Gender)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(r => r.BreedingStatus)
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(BreedingStatus.Available);
    }
}
