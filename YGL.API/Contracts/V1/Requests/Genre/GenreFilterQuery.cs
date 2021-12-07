using Microsoft.AspNetCore.Mvc;

namespace YGL.API.Contracts.V1.Requests.Genre; 

public class GenreFilterQuery {
    [FromQuery(Name = "genreName")] public string GenreName { get; set; }
}