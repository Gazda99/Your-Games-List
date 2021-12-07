using Microsoft.AspNetCore.Mvc;

namespace YGL.API.Contracts.V1.Requests.GameMode; 

public class GameModeFilterQuery {
    [FromQuery(Name = "gameModeName")] public string GameModeName { get; set; }
}