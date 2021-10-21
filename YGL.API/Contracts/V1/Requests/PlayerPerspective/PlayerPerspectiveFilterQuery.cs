using Microsoft.AspNetCore.Mvc;

namespace YGL.API.Contracts.V1.Requests.PlayerPerspective {
public class PlayerPerspectiveFilterQuery {
    [FromQuery(Name = "perspectiveName")] public string PlayerPerspectiveName { get; set; }
}
}