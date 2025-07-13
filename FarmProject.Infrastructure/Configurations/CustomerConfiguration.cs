using FarmProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmProject.Infrastructure.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(c => c.PhoneNum)
            .IsRequired()
            .HasMaxLength(16);

        builder.HasMany(c => c.Orders)
            .WithOne()
            .HasForeignKey("CustomerId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
