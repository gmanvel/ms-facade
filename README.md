# Partner Center Facade API

A .NET 10 minimal Web API that acts as a facade over the Microsoft Partner Center REST API.

## Prerequisites

- .NET 10 SDK
- Docker and Docker Compose (for containerized deployment)

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
dotnet restore
dotnet run --project PartnerCenterFacade
```

### With Docker Compose

```bash
docker-compose up --build
```

The API will be available at `http://localhost:5000`

## API Endpoints

- `GET /api/customers` - List all customers
- `GET /api/customers/{id}` - Get customer by ID
- `GET /api/subscriptions/{customerId}` - List subscriptions for a customer
- `GET /health` - Health check endpoint

## Project Structure

```
PartnerCenterFacade/
├── Clients/
│   ├── PartnerCenterClient.cs
│   └── PartnerCenterOptions.cs
├── Dtos/
│   ├── CustomerDto.cs
│   └── SubscriptionDto.cs
├── Program.cs
├── appsettings.json
└── appsettings.Development.json
```
