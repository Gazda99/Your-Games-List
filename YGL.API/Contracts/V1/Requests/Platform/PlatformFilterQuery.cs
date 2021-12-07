using Microsoft.AspNetCore.Mvc;

namespace YGL.API.Contracts.V1.Requests.Platform; 

public class PlatformFilterQuery {
    [FromQuery(Name = "platformName")] public string PlatformName { get; set; }

}