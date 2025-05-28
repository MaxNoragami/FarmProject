using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmProject.Infrastructure.Configurations;

public class PairConfiguration : IEntityTypeConfiguration<Pair>
{
    public void Configure(EntityTypeBuilder<Pair> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasOne(p => p.MaleRabbit)
            .WithMany()
            .HasForeignKey("MaleRabbitId")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.FemaleRabbit)
            .WithMany()
            .HasForeignKey("FemaleRabbitId")
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(p => p.StartDate)
            .IsRequired();

        builder.Property(p => p.EndDate)
            .IsRequired(false);

        builder.Property(p => p.PairingStatus)
           .IsRequired()
           .HasConversion<string>()
           .HasDefaultValue(PairingStatus.Active);
    }
}
