# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["src/Services/Customer/Customer.API/Customer.API.csproj", "src/Services/Customer/Customer.API/"]
COPY ["src/Services/Customer/Customer.Application/Customer.Application.csproj", "src/Services/Customer/Customer.Application/"]
COPY ["src/Services/Customer/Customer.Infrastructure/Customer.Infrastructure.csproj", "src/Services/Customer/Customer.Infrastructure/"]
COPY ["src/Services/Customer/Customer.Domain/Customer.Domain.csproj", "src/Services/Customer/Customer.Domain/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Common/BuildingBlocks.Common.csproj", "src/BuildingBlocks/BuildingBlocks.Common/"]
COPY ["src/BuildingBlocks/BuildingBlocks.EventBus/BuildingBlocks.EventBus.csproj", "src/BuildingBlocks/BuildingBlocks.EventBus/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Infrastructure/BuildingBlocks.Infrastructure.csproj", "src/BuildingBlocks/BuildingBlocks.Infrastructure/"]
COPY ["Directory.Packages.props", "./"]

# Restore
RUN dotnet restore "src/Services/Customer/Customer.API/Customer.API.csproj"

# Copy source code
COPY . .

# Build
RUN dotnet build "src/Services/Customer/Customer.API/Customer.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "src/Services/Customer/Customer.API/Customer.API.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 5003

ENTRYPOINT ["dotnet", "Customer.API.dll"]
