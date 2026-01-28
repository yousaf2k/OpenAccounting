using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Repositories;
using Identity.Infrastructure.Services;
using Identity.Domain.Entities;
using Identity.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using MediatR;
using FluentValidation;
using BuildingBlocks.Infrastructure.UnitOfWork;
using System.Text;
using OpenIddict.Validation.AspNetCore;

var builder = WebApplicationBuilder.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", "IdentityService");
});

// Add services
builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Identity.Infrastructure")));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.SignIn.RequireConfirmedEmail = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<IdentityDbContext>()
.AddDefaultTokenProviders();

// Configure OpenIddict (Comment 1)
builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore().UseDbContext<IdentityDbContext>();
    })
    .AddServer(options =>
    {
        // Enable authorization code flow with PKCE for SPAs
        options.AllowAuthorizationCodeFlow()
            .RequireProofKeyForCodeExchange();

        // Enable client credentials flow for service-to-service
        options.AllowClientCredentialsFlow();

        // Enable refresh token flow
        options.AllowRefreshTokenFlow();

        // Set endpoint URIs
        options.SetAuthorizationEndpointUris("/connect/authorize");
        options.SetTokenEndpointUris("/connect/token");
        options.SetUserinfoEndpointUris("/connect/userinfo");

        // Register the signing and encryption credentials
        options.AddDevelopmentEncryptionCertificate()
            .AddDevelopmentSigningCertificate();

        // Set token lifetimes
        options.SetAccessTokenLifetime(TimeSpan.FromHours(1));
        options.SetRefreshTokenLifetime(TimeSpan.FromDays(7));

        // Enable ASP.NET Core integration
        options.UseAspNetCore()
            .EnableAuthorizationEndpointPassthrough()
            .EnableTokenEndpointPassthrough()
            .EnableUserinfoEndpointPassthrough();
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });

// Add repositories
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<RefreshTokenRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork<IdentityDbContext>>();

// Add services
builder.Services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<CurrentUserService>();
builder.Services.AddScoped(sp =>
{
    var signingKey = builder.Configuration["JWT:SigningKey"];
    var issuer = builder.Configuration["JWT:Issuer"];
    var audience = builder.Configuration["JWT:Audience"];
    return new TokenService(signingKey!, issuer!, audience!);
});

// Configure JWT Authentication (Comment 3)
var jwtSettings = builder.Configuration.GetSection("JWT");
var signingKeyValue = jwtSettings["SigningKey"] ?? throw new InvalidOperationException("JWT:SigningKey not configured");
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKeyValue));
var issuerValue = jwtSettings["Issuer"] ?? "IdentityService";
var audienceValue = jwtSettings["Audience"] ?? "AccountingSystem";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingKey,
        ValidateIssuer = true,
        ValidIssuer = issuerValue,
        ValidateAudience = true,
        ValidAudience = audienceValue,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
    };
});

// Add Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("AdminOrAccountant", policy =>
        policy.RequireRole("Admin", "Accountant"));

    options.AddPolicy("AdminOrAccountantOrUser", policy =>
        policy.RequireRole("Admin", "Accountant", "User"));

    options.AddPolicy("AnyAuthenticated", policy =>
        policy.RequireAuthenticatedUser());
});

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Add FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

// Register OpenIddict seeder (Comment 1)
builder.Services.AddScoped<OpenIddictSeeder>();

// Register Role seeder (Comment 2)
builder.Services.AddScoped<RoleSeeder>();

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add controllers
builder.Services.AddControllers();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Add health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<IdentityDbContext>(tags: new[] { "services", "ready" });

// Add Swagger
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
    await dbContext.Database.MigrateAsync();

    // Seed OpenIddict applications and scopes (Comment 1)
    var seeder = scope.ServiceProvider.GetRequiredService<OpenIddictSeeder>();
    await seeder.SeedAsync();

    // Seed predefined roles with permissions (Comment 2)
    var roleSeeder = scope.ServiceProvider.GetRequiredService<RoleSeeder>();
    await roleSeeder.SeedAsync();
}

await app.RunAsync();
