---
description: Integrate a Microsoft Partner Center API endpoint from documentation URL (GET only)
argument-hint: <ms-docs-url>
---

You are integrating a new Microsoft Partner Center API endpoint into this facade application.

## Step 1: Fetch and Parse Documentation

Use WebFetch to retrieve the documentation at: $ARGUMENTS

Extract ONLY the REST API details (ignore C#, Python, PowerShell SDK examples):
- HTTP method
- URL pattern (e.g., `GET /{baseURL}/v1/customers/{customer-tenant-id}`)
- URI/route parameters with types
- Query string parameters (if any)
- Response body JSON structure

**IMPORTANT:** If the HTTP method is NOT GET, stop and respond:
"This command only supports GET endpoints. The documentation describes a [METHOD] endpoint."

## Step 2: Create Partner Center Response DTO

Create a new file in `PartnerCenterFacade/Clients/Models/` for the Partner Center API response.

Follow this pattern:
- Namespace: `PartnerCenterFacade.Clients.Models`
- Sealed class named `{Resource}Response`
- Include ALL fields from the API response JSON
- Use `string` with `= string.Empty` for required strings
- Use nullable types for optional fields (`string?`, `DateTimeOffset?`)

## Step 3: Create Facade DTO

Create a new file in `PartnerCenterFacade/Dtos/` for the facade response.

Follow this pattern:
- Namespace: `PartnerCenterFacade.Dtos`
- Sealed class named `{Resource}Dto`
- Include only the fields needed by facade consumers (can be subset of Response)
- Same property conventions as Response DTO

## Step 4: Add AutoMapper Mapping

Add a new mapping to `PartnerCenterFacade/Mapping/PartnerCenterMappingProfile.cs`:

```csharp
CreateMap<{Resource}Response, {Resource}Dto>();
```

## Step 5: Add Client Method

Add a new method to `PartnerCenterFacade/Clients/PartnerCenterClient.cs`:

Follow this pattern:
- Method name: `Get{Resource}Async` or `Get{Resource}By{Filter}Async`
- Parameters: route params + `CancellationToken ct = default`
- URL: `$"/{_options.ApiVersion}/..."` matching Partner Center URL
- Log with `_logger.LogInformation` using structured params
- Deserialize to `{Resource}Response`
- Use `_mapper.Map<{Resource}Dto>(response)` to map
- Return `Task<{Resource}Dto?>` for single items
- Return `Task<IReadOnlyList<{Resource}Dto>>` for collections

## Step 6: Register API Endpoint

Add endpoint to `PartnerCenterFacade/Program.cs` in the `api` MapGroup:

Follow this pattern:
```csharp
api.MapGet("/path/{param}", async (string param, PartnerCenterClient client, CancellationToken ct) =>
{
    var result = await client.Get{Resource}Async(param, ct);
    return result is null ? Results.NotFound() : Results.Ok(result);
})
.WithName("Get{Resource}")
.WithOpenApi();
```

## Step 7: Update CLAUDE.md

Add the new endpoint to the "API Endpoints" section in CLAUDE.md.

## Step 8: Update README.md

Add the new endpoint to the "API Endpoints" section in README.md, maintaining the same format as existing endpoints.

## Step 9: Build and Verify

Run `dotnet build` to verify there are no compilation errors.
