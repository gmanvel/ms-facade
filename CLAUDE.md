# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a .NET 10 minimal Web API that acts as a facade over the Microsoft Partner Center REST API. It provides simplified endpoints for accessing customer and subscription data.

## Development Commands

### Build and Run
```bash
# Restore dependencies
dotnet restore

# Run locally
dotnet run --project PartnerCenterFacade

# Build the project
dotnet build

# Publish for production
dotnet publish -c Release
```

### Docker
```bash
# Build and run with Docker Compose
docker-compose up --build

# Build Docker image manually
docker build -t partnercenter-facade .
```

## Configuration

The application requires Partner Center credentials configured via environment variables:

- `PC_CLIENT_ID` - Azure AD application client ID
- `PC_CLIENT_SECRET` - Azure AD application client secret
- `PC_TENANT_ID` - Azure AD tenant ID

These are mapped to the `PartnerCenter` configuration section in [appsettings.json](PartnerCenterFacade/appsettings.json).

## Architecture

### Configuration Pattern
The application uses the Options pattern for configuration. `PartnerCenterOptions` is bound from the `PartnerCenter` configuration section in [Program.cs:8-9](PartnerCenterFacade/Program.cs#L8-L9) and injected into `PartnerCenterClient`.

### HTTP Client Setup
`PartnerCenterClient` is registered with `AddHttpClient` in [Program.cs:12-17](PartnerCenterFacade/Program.cs#L12-L17), which:
- Configures the base URL from options
- Sets default headers for JSON responses
- Enables proper HttpClient lifecycle management

### Minimal API Pattern
Endpoints are registered directly in [Program.cs](PartnerCenterFacade/Program.cs) using the minimal API approach (no controllers). Each endpoint:
- Accepts `PartnerCenterClient` via dependency injection
- Uses `CancellationToken` for cancellation support
- Returns typed results (`Results.Ok`, `Results.NotFound`)
- Includes OpenAPI documentation via `.WithOpenApi()`

### Data Flow
1. HTTP request hits minimal API endpoint
2. Endpoint calls `PartnerCenterClient` method
3. Client constructs Partner Center API URL using `ApiVersion` from options
4. HttpClient sends request with configured base URL and headers
5. Response is deserialized to DTO and returned

### Key Components
- **PartnerCenterClient** ([Clients/PartnerCenterClient.cs](PartnerCenterFacade/Clients/PartnerCenterClient.cs)) - HTTP client wrapper that calls Microsoft Partner Center API
- **PartnerCenterOptions** ([Clients/PartnerCenterOptions.cs](PartnerCenterFacade/Clients/PartnerCenterOptions.cs)) - Configuration model for API credentials and base URL
- **DTOs** ([Dtos/](PartnerCenterFacade/Dtos/)) - Data transfer objects for API responses (CustomerDto, SubscriptionDto)

## API Endpoints

- `GET /api/customers` - List all customers
- `GET /api/customers/{id}` - Get specific customer
- `GET /api/subscriptions/{customerId}` - List customer subscriptions
- `GET /health` - Health check endpoint

## Local Development

The API runs on port 5000 when using `dotnet run`, or port 5001 when using Docker Compose (mapped to internal port 8080).
