using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using PartnerCenterFacade.Clients;

var builder = WebApplication.CreateBuilder(args);

// Configure PartnerCenter options
builder.Services.Configure<PartnerCenterOptions>(
    builder.Configuration.GetSection("PartnerCenter"));

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Register HttpClient for PartnerCenterClient
builder.Services.AddHttpClient<PartnerCenterClient>((sp, http) =>
{
    var opts = sp.GetRequiredService<IOptions<PartnerCenterOptions>>().Value;
    http.BaseAddress = new Uri(opts.BaseUrl);
    http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

// Add health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Map health check endpoint
app.MapHealthChecks("/health");

// Map API endpoints
var api = app.MapGroup("/api");

api.MapGet("/customers", async (PartnerCenterClient client, CancellationToken ct) =>
{
    var customers = await client.GetCustomersAsync(ct);
    return Results.Ok(customers);
})
.WithName("GetCustomers")
.WithOpenApi();

api.MapGet("/customers/{id}", async (string id, PartnerCenterClient client, CancellationToken ct) =>
{
    var customer = await client.GetCustomerByIdAsync(id, ct);
    return customer is null ? Results.NotFound() : Results.Ok(customer);
})
.WithName("GetCustomerById")
.WithOpenApi();

api.MapGet("/subscriptions/{customerId}", async (string customerId, PartnerCenterClient client, CancellationToken ct) =>
{
    var subs = await client.GetSubscriptionsByCustomerIdAsync(customerId, ct);
    return Results.Ok(subs);
})
.WithName("GetSubscriptionsByCustomerId")
.WithOpenApi();

app.Run();
