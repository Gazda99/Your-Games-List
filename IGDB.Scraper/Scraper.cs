using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        string type = typeof(T).FullName;

        int offset = 0;
        int currentCount = 0;

        Console.WriteLine("************************************************");
        Console.WriteLine($"Starting to deserializing {type} at {GetTime()}");
        Stopwatch sw = new Stopwatch();
        sw.Start();

        while (true) {
            SimpleHttpRequestMsg req = SimpleHttpRequestMsg
                .MethodAndUri(HttpMethod.Post, Url)
                .Headers(IGDBCredentials.HeadersDictionary)
                .Content(ContentBuilder(rawContent, limit, offset));

            //get response
            HttpResponseMessage res = await _httpClient.SendAsync(req);
            string stringRes = await res.Content.ReadAsStringAsync();


            Console.WriteLine($"Deserializing {type} {offset} - {offset + limit} ");

            //deserialize response
            List<T> deserializeObject =
                JsonConvert.DeserializeObject<List<T>>(stringRes,
                    JsonDeserializerSettings.DefaultJsonSerializerSettings);

            if (deserializeObject is null) Console.WriteLine("LIST IS NULL!!!!!");

            else {
                Console.WriteLine($"Deserialized item count: {deserializeObject.Count}");
                itemList.AddRange(deserializeObject);
                currentCount += deserializeObject.Count;
                Console.WriteLine($"Current count of deserialized items: {currentCount}");
            }

            offset += limit;

            string content = await res.Content.ReadAsStringAsync();
            int contentLength = content.Length;

            Console.WriteLine($"Response length: {contentLength}");

            //check if response is not null
            if (contentLength <= 5) {
                Console.WriteLine($"Finished before {offset} item");
                break;
            }


            Console.WriteLine($"Deserialized successfully at: {GetTime()}");
            Console.WriteLine("-----------------");

            Thread.Sleep(SleepTime);
        }

        Console.WriteLine($"Finished adding/updating {currentCount} items at: {GetTime()}");
        Console.WriteLine($"Operation took {sw.Elapsed.ToString()}");
        Console.WriteLine("************************************************");
        return itemList;
    }

    private static string GetTime() {
        return DateTime.Now.ToString("hh:mm:ss:ffff");
    }

    private static string ContentBuilder(string rawContent, int limit, int offset) {
        return $"{rawContent} limit {limit}; offset {offset};";
    }
}
}