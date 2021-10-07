namespace YGL.API.Contracts.V1.Responses {
public class GenericResponse<T> : IObjectForResponse {
    public T Value;
}
}