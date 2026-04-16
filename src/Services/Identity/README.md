# Identity Service

The Identity Service is a standalone microservice responsible for user authentication, authorization, and role management in the Accounting System. It implements OpenID Connect and OAuth2 patterns for secure API access.

## Overview

- **Architecture**: ASP.NET Core 10.0 with CQRS pattern (MediatR)
- **Authentication**: ASP.NET Core Identity with JWT tokens
- **Database**: SQL Server with Entity Framework Core
- **Event Bus**: RabbitMQ for inter-service communication
- **Logging**: Serilog with Seq integration
- **Tracing**: OpenTelemetry with Jaeger

## Key Features

- **User Registration**: Register new users with email validation
- **Login**: Authenticate users and issue JWT access and refresh tokens
- **Token Refresh**: Refresh expired access tokens using refresh tokens
- **Role Management**: Assign and remove roles for users
- **Password Management**: Change password and reset password flows
- **User Search**: Search and list users (Admin/Accountant only)
- **Account Management**: Activate and deactivate user accounts
- **Health Checks**: Database and service health monitoring

## Project Structure

```
Identity/
├── Identity.API/              # Web API project
│   ├── Controllers/          # API endpoints
│   ├── Program.cs            # Application entry point
│   ├── appsettings.json      # Configuration
│   └── Dockerfile            # Docker configuration
├── Identity.Application/      # Application layer
│   ├── Commands/             # CQRS commands
│   ├── Queries/              # CQRS queries
│   ├── Handlers/             # Command and query handlers
│   ├── DTOs/                 # Data transfer objects
│   ├── Validators/           # FluentValidation validators
│   └── IntegrationEvents/    # RabbitMQ events
├── Identity.Domain/          # Domain layer
│   ├── Entities/             # Domain entities
│   └── Events/               # Domain events
└── Identity.Infrastructure/  # Infrastructure layer
    ├── Persistence/          # DbContext
    ├── Repositories/         # Data repositories
    └── Services/             # Infrastructure services
```

## API Endpoints

### Authentication (`/api/auth`)

- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Authenticate user
- `POST /api/auth/refresh` - Refresh access token
- `POST /api/auth/change-password` - Change password
- `POST /api/auth/request-password-reset` - Request password reset
- `POST /api/auth/reset-password` - Reset password with token

### Users (`/api/users`)

- `GET /api/users` - Get all users (Admin/Accountant only)
- `GET /api/users/{id}` - Get user by ID
- `GET /api/users/me` - Get current user
- `PUT /api/users/{id}` - Update user (Admin or self)
- `DELETE /api/users/{id}` - Soft delete user (Admin only)
- `POST /api/users/{id}/activate` - Activate user (Admin only)
- `POST /api/users/{id}/deactivate` - Deactivate user (Admin only)
- `GET /api/users/search` - Search users (Admin/Accountant only)

### Roles (`/api/roles`)

- `GET /api/roles` - Get all roles (Admin only)
- `POST /api/roles` - Create role (Admin only)
- `PUT /api/roles/{id}` - Update role (Admin only)
- `DELETE /api/roles/{id}` - Delete role (Admin only)
- `POST /api/users/{userId}/roles` - Assign role to user (Admin only)
- `DELETE /api/users/{userId}/roles/{roleId}` - Remove role from user (Admin only)
- `GET /api/users/{userId}/roles` - Get user roles (Admin/Accountant or self)

## Configuration

### Environment Variables

| Variable | Description | Example |
|----------|-------------|---------|
| `ConnectionStrings__DefaultConnection` | SQL Server connection string | `Server=sqlserver;Database=IdentityDb;...` |
| `JWT__SigningKey` | Symmetric signing key (min 32 chars) | `your-secret-key-here-change-in-production` |
| `JWT__Issuer` | Token issuer URL | `http://identity-service` |
| `JWT__Audience` | Token audience | `accounting-api` |
| `ASPNETCORE_ENVIRONMENT` | Environment (Development/Production) | `Production` |
| `Serilog__WriteTo__0__Args__serverUrl` | Seq logging endpoint | `http://seq:5341` |

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=IdentityDb;Integrated Security=true;TrustServerCertificate=True;"
  },
  "JWT": {
    "SigningKey": "your-secret-key-here-change-in-production-min-32-chars",
    "Issuer": "http://localhost:5001",
    "Audience": "accounting-api"
  },
  "Serilog": {
    "MinimumLevel": "Information",
    "WriteTo": [
      { "Name": "Console" },
      { "Name": "Seq", "Args": { "serverUrl": "http://localhost:5341" } }
    ]
  }
}
```

## Authentication Flow

### Authorization Code Flow (Web Clients)

1. User navigates to login page
2. Frontend redirects to `/connect/authorize?client_id=...&redirect_uri=...&response_type=code&scope=openid+profile+email`
3. User enters credentials
4. Identity Service redirects back to frontend with authorization code
5. Frontend exchanges code for tokens via `/connect/token`
6. Frontend stores access token and refresh token
7. Frontend includes access token in API requests

### Client Credentials Flow (Service-to-Service)

1. Service sends credentials to `/connect/token`
2. Identity Service validates credentials
3. Identity Service returns access token
4. Service includes access token in API requests

## Password Requirements

- Minimum 8 characters
- At least one uppercase letter
- At least one lowercase letter
- At least one digit
- At least one special character

## Role Definitions

| Role | Permissions | Use Case |
|------|-------------|----------|
| **Admin** | All permissions | System administration |
| **Accountant** | Invoice, Payment, Reports, GL, AR, AP | Financial operations |
| **User** | Create/view invoices and payments, view reports | Regular users |
| **Viewer** | Read-only access to all resources | Auditors, read-only access |

## Database Migrations

### Initial Setup

```bash
# Create initial migration
dotnet ef migrations add InitialCreate \
  --project src/Services/Identity/Identity.Infrastructure \
  --startup-project src/Services/Identity/Identity.API

# Apply migration
dotnet ef database update \
  --project src/Services/Identity/Identity.Infrastructure \
  --startup-project src/Services/Identity/Identity.API
```

### Add New Migration

```bash
dotnet ef migrations add MigrationName \
  --project src/Services/Identity/Identity.Infrastructure \
  --startup-project src/Services/Identity/Identity.API

dotnet ef database update \
  --project src/Services/Identity/Identity.Infrastructure \
  --startup-project src/Services/Identity/Identity.API
```

## Development Setup

### Prerequisites

- .NET 10.0 SDK
- SQL Server
- Visual Studio Code or Visual Studio 2022+

### Local Development

1. Install dependencies:
```bash
dotnet restore
```

2. Update connection string in `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=IdentityDb;Integrated Security=true;TrustServerCertificate=True;"
  }
}
```

3. Apply migrations:
```bash
dotnet ef database update --project src/Services/Identity/Identity.Infrastructure --startup-project src/Services/Identity/Identity.API
```

4. Run the service:
```bash
dotnet run --project src/Services/Identity/Identity.API
```

5. Access Swagger UI at http://localhost:5001/swagger

## Testing

### Unit Tests

```bash
dotnet test src/Services/Identity/Identity.Application.Tests
dotnet test src/Services/Identity/Identity.Infrastructure.Tests
```

### Integration Tests

```bash
dotnet test src/Services/Identity/Identity.API.Tests
```

## Docker Deployment

### Build Image

```bash
docker build -f src/Services/Identity/Identity.API/Dockerfile -t accounting-identity-service .
```

### Run Container

```bash
docker run -d \
  --name identity-service \
  -p 5001:80 \
  -e ConnectionStrings__DefaultConnection="Server=sqlserver;Database=IdentityDb;User Id=sa;Password=..." \
  -e JWT__SigningKey="your-secret-key" \
  -e JWT__Issuer="http://identity-service" \
  -e JWT__Audience="accounting-api" \
  accounting-identity-service
```

## Security Considerations

- **Password Storage**: ASP.NET Core Identity uses PBKDF2 hashing
- **Token Security**: JWT with HS256 (HMAC-SHA256) signing
- **Token Expiry**: Access tokens expire after 1 hour, refresh tokens after 7 days
- **Token Revocation**: Refresh tokens can be revoked immediately
- **Account Lockout**: 5 failed login attempts trigger 15-minute lockout
- **HTTPS**: Required in production (enforced via middleware)
- **CORS**: Restricted to configured frontend origins only
- **Rate Limiting**: Configured in API Gateway (20 req/min for auth endpoints)

## Troubleshooting

### Connection String Issues

Ensure the connection string format is correct:
```
Server=sqlserver;Database=IdentityDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;
```

### JWT Token Issues

- Verify signing key is at least 32 characters
- Ensure issuer and audience match API Gateway configuration
- Check token expiry time

### Migration Issues

If migrations conflict:
```bash
# Remove last migration
dotnet ef migrations remove --project src/Services/Identity/Identity.Infrastructure --startup-project src/Services/Identity/Identity.API

# Reapply
dotnet ef database update --project src/Services/Identity/Identity.Infrastructure --startup-project src/Services/Identity/Identity.API
```

## Support

For issues or questions, refer to:
- [ASP.NET Core Identity Documentation](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- [MediatR Documentation](https://github.com/jbogard/MediatR)
- [FluentValidation Documentation](https://docs.fluentvalidation.net/)
