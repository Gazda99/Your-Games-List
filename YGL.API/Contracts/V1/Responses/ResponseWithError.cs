using System.Collections.Generic;

namespace YGL.API.Contracts.V1.Responses {
public class ResponseWithError<T> : IResponseWithErrors where T : IObjectForResponseWithErrors {
    public ResponseWithError() { }

    public ResponseWithError(T responseWithErrors) {
        ErrorMessages = responseWithErrors.ErrorMessages;
        ErrorCodes = responseWithErrors.ErrorCodes;
    }


    public IList<string> ErrorMessages { get; set; }
    public IList<int> ErrorCodes { get; set; }
}
}