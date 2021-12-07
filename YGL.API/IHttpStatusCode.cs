using System.Net;

namespace YGL.API; 

public interface IHttpStatusCode {
    public HttpStatusCode StatusCode { get; set; }
}