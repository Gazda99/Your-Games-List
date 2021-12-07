using Microsoft.AspNetCore.Mvc;

namespace YGL.API.Contracts.V1.Requests; 

public class PaginationQuery {
    public PaginationQuery() { }

    public PaginationQuery(int skip, int take) {
        Skip = skip;
        Take = take;
    }

    [FromQuery(Name = "skip")] public int Skip { get; set; }
    [FromQuery(Name = "take")] public int Take { get; set; }
}