using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Customer.Domain.Entities;

namespace Customer.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for the Customer entity.
/// </summary>
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasIndex(c => c.CustomerNumber).IsUnique();
        builder.HasIndex(c => c.Email);

        builder.Property(c => c.CustomerNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.CompanyName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.TaxId)
            .HasMaxLength(50);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.Phone)
            .HasMaxLength(20);

        builder.Property(c => c.Website)
            .HasMaxLength(255);

        builder.Property(c => c.CustomerType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(c => c.PaymentTerms)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(c => c.CreditLimit)
            .HasPrecision(18, 2)
            .HasDefaultValue(0);

        builder.Property(c => c.Notes)
            .HasMaxLength(1000);

        // Audit properties
        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.CreatedBy)
            .HasMaxLength(255);

        builder.Property(c => c.LastModifiedAt);

        builder.Property(c => c.LastModifiedBy)
            .HasMaxLength(255);

        // Soft delete
        builder.Property(c => c.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(c => c.DeletedAt);

        // Query filter for soft deletes
        builder.HasQueryFilter(c => !c.IsDeleted);

        // Navigation
        builder.HasMany(c => c.Contacts)
            .WithOne()
            .HasForeignKey(c => c.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Addresses)
            .WithOne()
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Customers");
    }
}

/// <summary>
/// Configuration for the Vendor entity.
/// </summary>
public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.HasKey(v => v.Id);

        builder.HasIndex(v => v.CompanyName);
        builder.HasIndex(v => v.Email);

        builder.Property(v => v.CompanyName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(v => v.TaxId)
            .HasMaxLength(50);

        builder.Property(v => v.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(v => v.Phone)
            .HasMaxLength(20);

        builder.Property(v => v.Website)
            .HasMaxLength(255);

        builder.Property(v => v.VendorType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(v => v.PaymentTerms)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(v => v.Notes)
            .HasMaxLength(1000);

        // Audit properties
        builder.Property(v => v.CreatedAt)
            .IsRequired();

        builder.Property(v => v.CreatedBy)
            .HasMaxLength(255);

        builder.Property(v => v.LastModifiedAt);

        builder.Property(v => v.LastModifiedBy)
            .HasMaxLength(255);

        // Soft delete
        builder.Property(v => v.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(v => v.DeletedAt);

        // Query filter for soft deletes
        builder.HasQueryFilter(v => !v.IsDeleted);

        // Navigation
        builder.HasMany(v => v.Contacts)
            .WithOne()
            .HasForeignKey(c => c.VendorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(v => v.Addresses)
            .WithOne()
            .HasForeignKey(a => a.VendorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Vendors");
    }
}

/// <summary>
/// Configuration for the Contact entity.
/// </summary>
public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.Phone)
            .HasMaxLength(20);

        builder.Property(c => c.Mobile)
            .HasMaxLength(20);

        builder.Property(c => c.Position)
            .HasMaxLength(100);

        builder.Property(c => c.IsPrimary)
            .HasDefaultValue(false);

        // Audit properties
        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.CreatedBy)
            .HasMaxLength(255);

        builder.Property(c => c.LastModifiedAt);

        builder.Property(c => c.LastModifiedBy)
            .HasMaxLength(255);

        // Soft delete
        builder.Property(c => c.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(c => c.DeletedAt);

        // Query filter for soft deletes
        builder.HasQueryFilter(c => !c.IsDeleted);

        builder.ToTable("Contacts");
    }
}

/// <summary>
/// Configuration for the Address entity.
/// </summary>
public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.AddressType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(a => a.Street1)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(a => a.Street2)
            .HasMaxLength(255);

        builder.Property(a => a.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.State)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.PostalCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(a => a.Country)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.IsPrimary)
            .HasDefaultValue(false);

        // Audit properties
        builder.Property(a => a.CreatedAt)
            .IsRequired();

        builder.Property(a => a.CreatedBy)
            .HasMaxLength(255);

        builder.Property(a => a.LastModifiedAt);

        builder.Property(a => a.LastModifiedBy)
            .HasMaxLength(255);

        // Soft delete
        builder.Property(a => a.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(a => a.DeletedAt);

        // Query filter for soft deletes
        builder.HasQueryFilter(a => !a.IsDeleted);

        builder.ToTable("Addresses");
    }
}
