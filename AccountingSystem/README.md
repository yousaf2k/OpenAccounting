# Accounting System

A comprehensive microservices-based accounting system built with .NET 10, following Clean Architecture and Domain-Driven Design principles.

## Architecture Overview

This solution implements a modular accounting system with the following components:

### API Gateway
- **YARP-based Reverse Proxy**: Single entry point for all client requests
- **Rate Limiting**: Protection against abuse with configurable policies
- **Authentication & Authorization**: JWT validation and role-based access control
- **Request Aggregation**: Backend-for-Frontend (BFF) pattern implementation
- **Health Monitoring**: Comprehensive health checks for all services
- **Service Discovery**: Dynamic service resolution
- **Observability**: Distributed tracing and centralized logging

### Microservices
- **Identity Service**: Authentication and authorization
- **Customer Service**: Customer management
- **Product Service**: Product catalog management
- **Invoice Service**: Invoice generation and management
- **Payment Service**: Payment processing
- **General Ledger Service**: Accounting ledger
- **AR/AP Services**: Accounts Receivable/Payable
- **Reporting Service**: Financial reporting
- **Document Service**: Document management

## Shared Infrastructure (BuildingBlocks)

The solution includes three shared infrastructure libraries:

### BuildingBlocks.Common
Core abstractions and utilities used across all microservices:
- Base entities with domain events
- DTOs for API responses
- Exception types
- Extension methods
- Validation constants

### BuildingBlocks.EventBus
Event-driven communication infrastructure:
- RabbitMQ and Kafka implementations
- Integration event abstractions
- Subscription management
- Resilience patterns

### BuildingBlocks.Infrastructure
Data access and repository patterns:
- Generic repository implementation
- Unit of Work pattern
- Entity Framework Core base context
- Audit and soft delete functionality

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd AccountingSystem
```

### 2. Start Infrastructure Services

```bash
docker-compose up -d
```

This will start the following services:
- **SQL Server**: Database server (localhost:1433)
- **RabbitMQ**: Message broker (localhost:5672, Management UI: localhost:15672)
- **Redis**: Caching server (localhost:6379)
- **Seq**: Centralized logging (localhost:5341)
- **Jaeger**: Distributed tracing (localhost:16686)

### 3. Build the Solution

```bash
dotnet build
```

### 4. Access Services and Infrastructure UIs

- **API Gateway**: http://localhost:5000 (main entry point)
- **API Gateway Health**: http://localhost:5000/health
- **RabbitMQ Management**: http://localhost:15672 (guest/guest)
- **Seq Logging**: http://localhost:5341
- **Jaeger Tracing**: http://localhost:16686

## Project Structure

```
AccountingSystem/
├── src/
│   ├── ApiGateway/           # YARP-based API Gateway
│   ├── BuildingBlocks/
│   │   ├── Common/           # Shared utilities and base classes
│   │   ├── EventBus/         # Message broker abstractions
│   │   └── Infrastructure/   # Data access patterns
│   └── Services/             # Microservices (future)
├── tests/                    # Test projects
├── docker/                   # Docker-related files
├── docs/                     # Documentation
├── AccountingSystem.sln      # Solution file
├── docker-compose.yml        # Infrastructure services
├── docker-compose.override.yml # Development overrides
└── README.md
```

## Development Workflow

### Adding a New Microservice

1. Create a new class library project in `src/Services/`
2. Reference the BuildingBlocks libraries
3. Implement domain models inheriting from base entities
4. Configure dependency injection using the provided extensions
5. Add database migrations for the service's bounded context

### Database Migrations

Each microservice manages its own database schema:

```bash
# In the microservice project directory
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Event-Driven Communication

Services communicate asynchronously using the event bus:

```csharp
// Publishing an event
await _eventBus.PublishAsync(new InvoiceCreatedEvent(invoiceId));

// Subscribing to events
await _eventBus.SubscribeAsync<InvoiceCreatedEvent, InvoiceCreatedEventHandler>();
```

## Configuration

### Connection Strings

Update connection strings in `appsettings.json` for each microservice:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=AccountingDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True"
  },
  "RabbitMQ": {
    "HostName": "localhost",
    "UserName": "guest",
    "Password": "guest"
  },
  "Redis": {
    "ConnectionString": "localhost:6379"
  }
}
```

## Testing

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## Deployment

### Docker Build

```bash
# Build all services
docker-compose -f docker-compose.yml -f docker-compose.override.yml build

# Run in production mode
docker-compose -f docker-compose.yml up -d
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests
5. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For questions and support, please open an issue on GitHub.