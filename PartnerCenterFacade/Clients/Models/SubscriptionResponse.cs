namespace PartnerCenterFacade.Clients.Models;

public sealed class SubscriptionResponse
{
    public string Id { get; set; } = string.Empty;
    public string OfferId { get; set; } = string.Empty;
    public string SkuId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTimeOffset? CreationDate { get; set; }
    public DateTimeOffset? ExpirationDate { get; set; }
}
