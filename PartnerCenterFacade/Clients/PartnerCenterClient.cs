using System.Net.Http.Json;
using AutoMapper;
using Microsoft.Extensions.Options;
using PartnerCenterFacade.Clients.Models;
using PartnerCenterFacade.Dtos;

namespace PartnerCenterFacade.Clients;

public sealed class PartnerCenterClient
{
    private readonly HttpClient _httpClient;
    private readonly PartnerCenterOptions _options;
    private readonly ILogger<PartnerCenterClient> _logger;
    private readonly IMapper _mapper;

    public PartnerCenterClient(HttpClient httpClient, IOptions<PartnerCenterOptions> options, ILogger<PartnerCenterClient> logger, IMapper mapper)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CustomerDto>> GetCustomersAsync(CancellationToken ct = default)
    {
        var url = $"/{_options.ApiVersion}/customers";
        _logger.LogInformation("Fetching customers from {Url}", url);
        var response = await _httpClient.GetFromJsonAsync<List<CustomerResponse>>(url, ct);
        return response is null
            ? []
            : _mapper.Map<List<CustomerDto>>(response);
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(string id, CancellationToken ct = default)
    {
        var url = $"/{_options.ApiVersion}/customers/{id}";
        _logger.LogInformation("Fetching customer {CustomerId} from {Url}", id, url);
        var response = await _httpClient.GetFromJsonAsync<CustomerResponse>(url, ct);
        return response is null ? null : _mapper.Map<CustomerDto>(response);
    }

    public async Task<IReadOnlyList<SubscriptionDto>> GetSubscriptionsByCustomerIdAsync(string customerId, CancellationToken ct = default)
    {
        var url = $"/{_options.ApiVersion}/customers/{customerId}/subscriptions";
        _logger.LogInformation("Fetching subscriptions for customer {CustomerId} from {Url}", customerId, url);
        var response = await _httpClient.GetFromJsonAsync<List<SubscriptionResponse>>(url, ct);
        return response is null
            ? []
            : _mapper.Map<List<SubscriptionDto>>(response);
    }
}
