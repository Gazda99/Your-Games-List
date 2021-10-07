using System.Collections.Generic;
using System.Net;
using YGL.API.Domain.SafeObjects;
using YGL.API.Errors;

namespace YGL.API.Domain {
public class CompanyResult : IErrorList, IHttpStatusCode {
    public IList<string> ErrorMessages { get; set; }
    public IList<int> ErrorCodes { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccess { get; set; }

    public ICollection<SafeCompany> Companies { get; set; }
}
}