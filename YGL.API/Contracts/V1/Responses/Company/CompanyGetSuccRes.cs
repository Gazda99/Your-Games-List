using YGL.API.Domain.SafeObjects;

namespace YGL.API.Contracts.V1.Responses.Company {
public class CompanyGetSuccRes : IObjectForResponse {
    
    public SafeCompany Company { get; set; }
}
}