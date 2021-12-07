using Microsoft.AspNetCore.Mvc;

namespace YGL.API.Contracts.V1.Requests.User; 

public class UserFilterQuery {
    [FromQuery(Name = "username")] public string Username { get; set; }
    [FromQuery(Name = "showInactive")] public bool ShowInactive { get; set; }
}