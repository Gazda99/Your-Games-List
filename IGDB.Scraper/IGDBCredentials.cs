using System.Collections.Generic;
using System.IO;

namespace IGDB.Scraper {
public static class IGDBCredentials {
    private static readonly string ClientId = File.ReadAllLines("../../../../Twitch Data.txt")[0];
    private static readonly string Auth = File.ReadAllLines("../../../../Twitch Data.txt")[1];

    public static readonly Dictionary<string, string> HeadersDictionary = new Dictionary<string, string> {
        { "Client-ID", ClientId },
        { "Authorization", Auth }
    };
}
}