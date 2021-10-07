using System.Collections.Generic;

namespace YGL.API.Contracts.V1.Responses {
public class PagedResponse<T> : IResponse where T : IObjectForResponse {
    public PagedResponse() { }

    public PagedResponse(IEnumerable<T> data) {
        Data = data;
    }

    public IEnumerable<T> Data { get; set; }
    public int? Amount { get; set; }

    public int? Skipped { get; set; }

    public int NextAt { get; set; }
    // public string NextPage { get; set; }
    // public string PrevPage { get; set; }
}
}