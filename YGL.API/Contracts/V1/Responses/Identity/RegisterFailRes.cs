using System.Collections.Generic;
using YGL.API.Errors;

namespace YGL.API.Contracts.V1.Responses.Identity; 

public class RegisterFailRes : IObjectForResponseWithErrors {
    public IList<string> ErrorMessages { get; set; }
    public IList<int> ErrorCodes { get; set; }
}