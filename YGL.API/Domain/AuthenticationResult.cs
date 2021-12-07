using System.Collections.Generic;
using System.Net;
using YGL.API.Errors;

namespace YGL.API.Domain; 

public class AuthenticationResult : IErrorList, IHttpStatusCode {
    public long UserId { get; set; }
    public string JwtToken { get; set; }
    public string RefreshToken { get; set; }

    public bool IsSuccess { get; set; }

    public IList<string> ErrorMessages { get; set; }
    public IList<int> ErrorCodes { get; set; }

    public HttpStatusCode StatusCode { get; set; }

    public string Location { get; set; }
}