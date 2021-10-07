using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IGDB.Scraper {
public class Scraper {
    private const int SleepTime = 150;

    private readonly SimpleHttpClient _httpClient = new SimpleHttpClient();
    public string Url { get; set; }

    public async Task<List<T>> Scrape<T>(string rawContent, int limit) {
        List<T> itemList = new List<T>();

        int offset = 0;

        while (true) {
            SimpleHttpRequestMsg req = SimpleHttpRequestMsg
                .MethodAndUri(HttpMethod.Post, Url)
                .Headers(IGDBCredentials.HeadersDictionary)
                .Content(ContentBuilder(rawContent, limit, offset));

            //get response
            var res = await _httpClient.SendAsync(req);
            var stringRes = await res.Content.ReadAsStringAsync();


            Console.WriteLine($"Deserializing {typeof(T).FullName} {offset} - {offset + limit} ");

            //deserialize response
            List<T> deserializeObject =
                JsonConvert.DeserializeObject<List<T>>(stringRes,
                    JsonDeserializerSettings.DefaultJsonSerializerSettings);

            if (deserializeObject is null) Console.WriteLine("LIST IS NULL!!!!!");

            else {
                Console.WriteLine($"Deserialized item count: {deserializeObject.Count}");
                itemList.AddRange(deserializeObject);
            }

            offset += limit;

            string content = await res.Content.ReadAsStringAsync();
            int contentLength = content.Length;

            Console.WriteLine($"Response length: {contentLength}");

            //check if response is not null

            if (contentLength <= 2) {
                Console.WriteLine($"Finished before {offset}");
                break;
            }


            Console.WriteLine(
                $"Deserialized successfully at: {DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}");
            Console.WriteLine();

            Thread.Sleep(SleepTime);
        }

        return itemList;
    }


    private static string ContentBuilder(string rawContent, int limit, int offset) {
        return $"{rawContent} limit {limit}; offset {offset};";
    }
}
}