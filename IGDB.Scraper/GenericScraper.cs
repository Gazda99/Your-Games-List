using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IGDB.Scraper {
public class GenericScraper {
    private readonly SimpleHttpClient _httpClient = new SimpleHttpClient();
    public string Url { get; set; }

    private int _currentCount = 0;

    /// <summary>
    /// Minimum delay beetween requests
    /// </summary>
    public long DelayBetweenRequests { get; set; } = 1000;

    /// <summary>
    /// Generic method for downloading  
    /// </summary>
    /// <param name="rawContent">Query that will be send to IGDB Api. Limit, offset and other parameters will be added later</param>
    /// <param name="lop">How many queries will be executed at one time.</param>
    /// <param name="limit">How many items will be requested in query</param>
    public async Task<List<T>> Scrape<T>(string rawContent, int lop, int limit) {
        string type = typeof(T).FullName;
        bool condition = true;
        int offset = 0;

        List<T> finalList = new List<T>();

        Console.WriteLine("\n\n************************************************");
        Console.WriteLine($"Starting to deserializing {type} at {GetTime()}");
        Console.WriteLine("************************************************");
        Stopwatch totalSw = new Stopwatch();
        totalSw.Start();

        int taskId = 0;

        while (condition) {
            List<Task<ScrapeResult<T>>> tasks = new List<Task<ScrapeResult<T>>>();

            Stopwatch partSw = new Stopwatch();
            partSw.Start();

            for (int i = 0; i < lop; i++) {
                tasks.Add(ScrapeBatch<T>(rawContent, offset, limit, taskId));
                offset += limit;
                taskId++;
            }

            IEnumerable<ScrapeResult<T>> tasksResults = await Task.WhenAll(tasks);

            foreach (ScrapeResult<T> scrapedResult in tasksResults) {
                if (!scrapedResult.IsSuccess)
                    condition = false;
                else
                    finalList.AddRange(scrapedResult.ScrapedItems);
            }

            long requestsTime = partSw.ElapsedMilliseconds;
            partSw.Stop();

            long diff = DelayBetweenRequests - requestsTime;

            if (diff > 0)
                await Task.Delay((int)diff + 10);
        }

        TimeSpan timeSpan = TimeSpan.FromMilliseconds(totalSw.ElapsedMilliseconds);
        totalSw.Stop();

        Console.WriteLine("************************************************");
        Console.WriteLine($"Finished scraping {_currentCount} {type} at: {GetTime()}");
        Console.WriteLine($"Operation took {timeSpan.Minutes}m:{timeSpan.Seconds}s:{timeSpan.Milliseconds}ms");
        Console.WriteLine("************************************************\n\n");

        return finalList;
    }

    /// <summary>
    /// Sends the request to IGDB Api and returns list of deserialized objects. 
    /// </summary>
    private async Task<ScrapeResult<T>> ScrapeBatch<T>(string rawContent, int offset, int limit, int taskId) {
        ScrapeResult<T> scrapeResult = new ScrapeResult<T>();

        string type = typeof(T).FullName;

        SimpleHttpRequestMsg req = SimpleHttpRequestMsg
            .MethodAndUri(HttpMethod.Post, Url)
            .Headers(IGDBCredentials.HeadersDictionary)
            .Content(ContentBuilder(rawContent, limit, offset));

        //get response
        HttpResponseMessage res = await _httpClient.SendAsync(req);
        string stringRes = await res.Content.ReadAsStringAsync();

        StringBuilder sb = new StringBuilder();

        sb.Append($"Task {taskId} trying to deserialize {type} {offset} - {offset + limit} \n");

        List<T> deserializeObjects =
            JsonConvert.DeserializeObject<List<T>>(stringRes, JsonDeserializerSettings.DefaultJsonSerializerSettings);

        if (deserializeObjects is null) {
            scrapeResult.IsSuccess = false;
            sb.Append("LIST IS NULL!!!!!\n");
        }

        else {
            sb.Append($"Deserialized item count in this batch: {deserializeObjects.Count}\n");
            scrapeResult.ScrapedItems = deserializeObjects;
            _currentCount += deserializeObjects.Count;
            sb.Append($"Current total count of deserialized items: {_currentCount}\n");
        }

        string content = await res.Content.ReadAsStringAsync();
        int contentLength = content.Length;

        if (contentLength <= 5) {
            sb.Append($"Finished before {offset} item\n");
            scrapeResult.IsSuccess = false;
        }

        sb.Append($"Task {taskId} finished at: {GetTime()}\n");
        sb.Append("-----------------\n");

        Console.WriteLine(sb.ToString());

        return scrapeResult;
    }

    private static string GetTime() {
        return DateTime.Now.ToString("HH:mm:ss:ff");
    }

    private static string ContentBuilder(string rawContent, int limit, int offset) {
        return $"{rawContent} limit {limit}; offset {offset};";
    }
}
}