using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BuildingBlocks.Infrastructure.Persistence;
using OpenIddict.EntityFrameworkCore.Models;

namespace Identity.Infrastructure.Persistence;

/// <summary>
/// DbContext for the Identity Service.
/// </summary>
public class IdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityDbContext"/> class.
    /// </summary>
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the refresh tokens.
    /// </summary>
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure OpenIddict entities (Comment 1)
        modelBuilder.UseOpenIddict<OpenIddictEntityFrameworkCoreApplication,
            OpenIddictEntityFrameworkCoreAuthorization,
            OpenIddictEntityFrameworkCoreScope,
            OpenIddictEntityFrameworkCoreToken,
            Guid>();

        ConfigureApplicationUser(modelBuilder);
        ConfigureApplicationRole(modelBuilder);
        ConfigureRefreshToken(modelBuilder);
    }

    private static void ConfigureApplicationUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationUser>(builder =>
        {
            builder.Property(u => u.FirstName)
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .HasMaxLength(100);

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);

            builder.HasIndex(u => u.IsActive);
        });

        // Add audit and soft delete properties
        modelBuilder.Entity<ApplicationUser>().Property<DateTime>("CreatedAt");
        modelBuilder.Entity<ApplicationUser>().Property<string?>("CreatedBy");
        modelBuilder.Entity<ApplicationUser>().Property<DateTime?>("LastModifiedAt");
        modelBuilder.Entity<ApplicationUser>().Property<string?>("LastModifiedBy");
        modelBuilder.Entity<ApplicationUser>().Property<DateTime?>("DeletedAt");
        modelBuilder.Entity<ApplicationUser>().Property<bool>("IsDeleted");

        modelBuilder.Entity<ApplicationUser>()
            .HasIndex(u => u.UserName)
            .IsUnique();

        modelBuilder.Entity<ApplicationUser>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }

    private static void ConfigureApplicationRole(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationRole>(builder =>
        {
            builder.Property(r => r.Description)
                .HasMaxLength(500);

            builder.Property(r => r.Permissions)
                .HasColumnType("nvarchar(max)");

            builder.HasIndex(r => r.Name).IsUnique();
        });
    }

    private static void ConfigureRefreshToken(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>(builder =>
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Token)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(rt => rt.UserId)
                .IsRequired();

            builder.Property(rt => rt.ExpiresAt)
                .IsRequired();

            builder.Property(rt => rt.CreatedAt)
                .IsRequired();

            builder.Property(rt => rt.RevokedAt);

            builder.Property(rt => rt.ReplacedByToken);

            builder.HasIndex(rt => rt.Token).IsUnique();
            builder.HasIndex(rt => rt.UserId);
            builder.HasIndex(rt => rt.IsRevoked);
            builder.HasIndex(rt => rt.ExpiresAt);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
