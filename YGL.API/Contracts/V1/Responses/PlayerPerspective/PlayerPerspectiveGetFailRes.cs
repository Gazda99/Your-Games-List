using System.Collections.Generic;

namespace YGL.API.Contracts.V1.Responses.PlayerPerspective; 

public class PlayerPerspectiveGetFailRes : IObjectForResponseWithErrors {
    public IList<string> ErrorMessages { get; set; }
    public IList<int> ErrorCodes { get; set; }
}