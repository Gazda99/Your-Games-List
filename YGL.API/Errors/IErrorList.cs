using System.Collections.Generic;

namespace YGL.API.Errors {
public interface IErrorList {
    public IList<string> ErrorMessages { get; set; }
    public IList<int> ErrorCodes { get; set; }
}
}