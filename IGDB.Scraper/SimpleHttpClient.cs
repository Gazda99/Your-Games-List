using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IGDB.Scraper; 

public class SimpleHttpClient {
    private readonly HttpClient _client = new HttpClient();

    ~SimpleHttpClient() {
        _client.Dispose();
    }

    /// <summary>
    /// Sets the base address of HttpClient, if passed argument is null or not valid Uri format then sets is to null
    /// </summary>
    public string BaseAddress {
        get => _client.BaseAddress?.ToString();
        set {
            try {
                _client.BaseAddress = value is not null ? new Uri(value, UriKind.Absolute) : null;
            }
            catch (Exception e) when (e is ArgumentException or UriFormatException) {
                Console.WriteLine(e);
                _client.BaseAddress = null;
            }
        }
    }

    /// <summary>
    /// Sens the HttpRequests
    /// </summary>
    /// <returns>Server response as HttpResponseMessage</returns>
    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage req) {
        HttpResponseMessage res = await _client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead);
        return res;
    }

    public async Task<HttpResponseMessage> SendAsync(SimpleHttpRequestMsg req) =>
        await SendAsync((HttpRequestMessage)req);
}