using System.Collections.Generic;
using YGL.API.Domain;
using YGL.API.SafeObjects;

namespace YGL.API.Contracts.V1.Responses.User; 

public class UserGetSuccRes : IObjectForResponse {
    public SafeUser User { get; set; }
}