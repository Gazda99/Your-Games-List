using System.Collections.Generic;

namespace YGL.API.Contracts.V1.Responses.GameMode; 

public class GameModeGetFailRes : IObjectForResponseWithErrors 
{
    public IList<string> ErrorMessages { get; set; }
    public IList<int> ErrorCodes { get; set; }
}