# Partner Center Facade API

A .NET 10 minimal Web API that acts as a facade over the Microsoft Partner Center REST API. This facade simplifies access to Partner Center data by providing clean, focused endpoints for customer and subscription management.

## Features

- Minimal API design for lightweight, high-performance endpoints
- Strongly-typed configuration using the Options pattern
- AutoMapper for clean separation between API responses and DTOs
- Built-in health checks
- Docker support for easy deployment
- OpenAPI/Swagger documentation

## Prerequisites

- .NET 10 SDK
- Docker and Docker Compose (for containerized deployment)
- Microsoft Partner Center API credentials

## Configuration

Set the following environment variables:

```bash
export PC_CLIENT_ID="your-client-id"
export PC_CLIENT_SECRET="your-client-secret"
export PC_TENANT_ID="your-tenant-id"
```

## Running Locally

### With .NET CLI

```bash
# Restore dependencies
dotnet restore

# Run the application
dotnet run --project PartnerCenterFacade

# Build the project
dotnet build

# Publish for production
dotnet publish -c Release
```

The API will be available at `http://localhost:5000`

### With Docker Compose

```bash
docker-compose up --build
```

The API will be available at `http://localhost:5001` (mapped from internal port 8080)

## API Endpoints

- `GET /api/customers` - List all customers
- `GET /api/customers/{id}` - Get customer by ID
- `GET /api/subscriptions/{customerId}` - List subscriptions for a customer
- `GET /health` - Health check endpoint

## Architecture

This application follows a clean, layered architecture:

- **Minimal API Pattern**: Endpoints are defined directly in `Program.cs` using .NET's minimal API approach
- **Options Pattern**: Configuration is strongly-typed using `PartnerCenterOptions` bound from `appsettings.json`
- **Dependency Injection**: `PartnerCenterClient` is registered with `AddHttpClient` for proper lifecycle management
- **AutoMapper**: Response models from Partner Center API are mapped to simplified DTOs for the facade

### Data Flow

1. HTTP request hits minimal API endpoint
2. Endpoint calls `PartnerCenterClient` method
3. Client constructs Partner Center API URL using configured API version
4. HttpClient sends request to Partner Center with authentication headers
5. Response is deserialized to Response model
6. AutoMapper maps Response to DTO
7. DTO is returned to client

## Project Structure

```
PartnerCenterFacade/
├── Clients/
│   ├── Models/                         # Partner Center API response models
│   │   ├── CustomerResponse.cs
│   │   └── SubscriptionResponse.cs
│   ├── PartnerCenterClient.cs          # HTTP client for Partner Center API
│   └── PartnerCenterOptions.cs         # Configuration options
├── Dtos/                                # Facade DTOs for API responses
│   ├── CustomerDto.cs
│   └── SubscriptionDto.cs
├── Mapping/
│   └── PartnerCenterMappingProfile.cs  # AutoMapper configuration
├── Program.cs                           # Application entry point and endpoint definitions
├── appsettings.json
└── appsettings.Development.json
```

## Adding New Endpoints

To integrate a new Partner Center API endpoint, use the `/integrate-endpoint` command:

```bash
/integrate-endpoint <microsoft-docs-url>
```

This command automates the process of:
1. Parsing API documentation
2. Creating Response and DTO models
3. Adding AutoMapper mappings
4. Implementing client methods
5. Registering API endpoints
6. Updating documentation

## Technology Stack

- **.NET 10**: Latest .NET framework with minimal API support
- **AutoMapper**: Object-to-object mapping for clean DTOs
- **System.Net.Http.Json**: Built-in JSON serialization
- **Docker**: Containerization for deployment

## Development

For detailed development guidelines and architecture documentation, see [CLAUDE.md](CLAUDE.md).
