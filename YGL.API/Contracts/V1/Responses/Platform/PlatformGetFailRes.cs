using System.Collections.Generic;

namespace YGL.API.Contracts.V1.Responses.Platform {
public class PlatformGetFailRes : IObjectForResponseWithErrors{
    public IList<string> ErrorMessages { get; set; }
    public IList<int> ErrorCodes { get; set; }
}
}