using System.Collections.Generic;
using System.Net.Http;

namespace IGDB.Scraper {
public class SimpleHttpRequestMsg {
    private readonly HttpRequestMessage _req;

    /// <summary>
    /// MyHttpRequestMessage constructor
    /// </summary>
    private SimpleHttpRequestMsg(HttpMethod method, string uri) {
        _req = new HttpRequestMessage(method, uri);
    }

    public static explicit operator HttpRequestMessage(SimpleHttpRequestMsg value) => value._req;

    /// <summary>
    /// Creates new SimpleHttpRequestMsg instance with given HttpMethod and Uri
    /// </summary>
    /// <returns>New SimpleHttpRequestMsg instance</returns>
    public static SimpleHttpRequestMsg MethodAndUri(HttpMethod method, string uri) {
        SimpleHttpRequestMsg sHttpReq = new SimpleHttpRequestMsg(method, uri);
        return sHttpReq;
    }


    /// <summary>
    /// Sets the Headers of HttpRequestMessage
    /// </summary>
    /// <returns>SimpleHttpRequestMsg instance</returns>
    public SimpleHttpRequestMsg Headers(Dictionary<string, string> headerDict) {
        if (headerDict is null) return this;
        foreach (var (key, value) in headerDict)
            _req.Headers.Add(key, value);
        return this;
    }

    /// <summary>
    /// Sets the Content of HttpRequestMessage using FormUrlEncoded
    /// </summary>
    /// <returns>SimpleHttpRequestMsg instance</returns>
    public SimpleHttpRequestMsg Content(Dictionary<string, string> data) {
        FormUrlEncodedContent content = new FormUrlEncodedContent(data);
        _req.Content = content;
        return this;
    }

    /// <summary>
    /// Sets the Headers of HttpRequestMessage using StringContent
    /// </summary>
    /// <returns>SimpleHttpRequestMsg instance</returns>
    public SimpleHttpRequestMsg Content(string data) {
        StringContent content = new StringContent(data);
        _req.Content = content;
        return this;
    }
}
}