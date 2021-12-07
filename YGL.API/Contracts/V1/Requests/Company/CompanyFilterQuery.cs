using Microsoft.AspNetCore.Mvc;

namespace YGL.API.Contracts.V1.Requests.Company; 

public class CompanyFilterQuery {
    [FromQuery(Name = "companyName")] public string CompanyName { get; set; }
}