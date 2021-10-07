using System.Collections.Generic;
using System.Net;
using YGL.API.Errors;

namespace YGL.API.Domain {
public class EmailConfirmationResult : IErrorList, IHttpStatusCode {
    public bool IsSuccess { get; set; }
    public IList<string> ErrorMessages { get; set; }
    public IList<int> ErrorCodes { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    
}
}