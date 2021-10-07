using System.Collections.Generic;

namespace YGL.API.Contracts.V1.Responses.Identity {
public class EmailConfirmationFailRes : IObjectForResponseWithErrors {
    public IList<string> ErrorMessages { get; set; }
    public IList<int> ErrorCodes { get; set; }
}
}