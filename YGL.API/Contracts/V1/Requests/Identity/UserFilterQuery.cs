using Microsoft.AspNetCore.Mvc;

namespace YGL.API.Contracts.V1.Requests.Identity {
public class UserFilterQuery {
    [FromQuery(Name = "username")] public string Username { get; set; }
}
}