using System.Collections.Generic;

namespace YGL.API.Contracts.V1.Responses; 

public class Response<T> : IResponse where T : IObjectForResponse {
    public Response() { }

    public Response(IEnumerable<T> data) {
        Data = data;
    }

    public IEnumerable<T> Data { get; set; }

    public int? Amount { get; set; }
}