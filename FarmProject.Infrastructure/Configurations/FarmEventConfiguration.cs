using FarmProject.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmProject.Infrastructure.Configurations;

public class FarmEventConfiguration : IEntityTypeConfiguration<FarmEvent>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<FarmEvent> builder)
    {
        builder.HasKey(fe => fe.Id);

        builder.Property(fe => fe.FarmEventType)
            .IsRequired();

        builder.Property(fe => fe.Message)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(fe => fe.IsCompleted)
            .IsRequired();

        builder.Property(fe => fe.CreatedOn)
            .IsRequired();

        builder.Property(fe => fe.DueOn)
            .IsRequired();

    }
}
