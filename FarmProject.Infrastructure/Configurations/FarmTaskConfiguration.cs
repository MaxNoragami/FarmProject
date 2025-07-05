using FarmProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmProject.Infrastructure.Configurations;

public class FarmTaskConfiguration : IEntityTypeConfiguration<FarmTask>
{
    public void Configure(EntityTypeBuilder<FarmTask> builder)
    {
        builder.HasKey(fe => fe.Id);

        builder.Property(fe => fe.FarmTaskType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(fe => fe.Message)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(fe => fe.IsCompleted)
            .IsRequired();

        builder.Property(fe => fe.CreatedOn)
            .IsRequired();

        builder.Property(fe => fe.DueOn)
            .IsRequired();

        builder.Property(t => t.CageId)
            .IsRequired(false);
    }
}
