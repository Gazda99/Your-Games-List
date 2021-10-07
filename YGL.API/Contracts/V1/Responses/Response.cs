namespace YGL.API.Contracts.V1.Responses {
public class Response<T> : IResponse where T : IObjectForResponse {
    public Response() { }

    public Response(T data) {
        Data = data;
    }

    public T Data { get; set; }
}
}