namespace YGL.API.Contracts.V1.Responses; 

public class SingleResponse<T> : IResponse where T : IObjectForResponse {
    public SingleResponse() { }

    public SingleResponse(T data) {
        Data = data;
    }

    public T Data { get; set; }
}