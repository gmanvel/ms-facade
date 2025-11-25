using AutoMapper;
using PartnerCenterFacade.Clients.Models;
using PartnerCenterFacade.Dtos;

namespace PartnerCenterFacade.Mapping;

public class PartnerCenterMappingProfile : Profile
{
    public PartnerCenterMappingProfile()
    {
        CreateMap<CustomerResponse, CustomerDto>();
        CreateMap<SubscriptionResponse, SubscriptionDto>();
    }
}
