using System.Collections.Generic;
using YGL.API.Contracts.V1.Responses;

namespace YGL.API.Contracts;

public class DbConnectionErrorRes : IObjectForResponseWithErrors {
    public IList<string> ErrorMessages { get; set; }
    public IList<int> ErrorCodes { get; set; }
}