using System.Collections.Generic;

namespace YGL.API.Contracts.V1.Responses.Company; 

public class CompanyGetFailRes : IObjectForResponseWithErrors {
    public IList<string> ErrorMessages { get; set; }
    public IList<int> ErrorCodes { get; set; }
}