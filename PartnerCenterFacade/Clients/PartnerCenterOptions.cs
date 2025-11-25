namespace PartnerCenterFacade.Clients;

public sealed class PartnerCenterOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public string ApiVersion { get; set; } = "v1";
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
}
