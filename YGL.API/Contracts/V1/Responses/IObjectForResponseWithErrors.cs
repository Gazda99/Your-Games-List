using YGL.API.Errors;

namespace YGL.API.Contracts.V1.Responses; 

public interface IObjectForResponseWithErrors : IObjectForResponse, IErrorList { }