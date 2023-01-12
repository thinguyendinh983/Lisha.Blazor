using AutoMapper;
using Lisha.Blazor.Infrastructure.ApiClient;
using static Lisha.Blazor.Pages.Personal.AuditLogs;

namespace Lisha.Blazor.Mappings.Auditing
{
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            CreateMap<AuditDto, RelatedAuditTrail>();
        }
    }
}
