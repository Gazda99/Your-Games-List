using System.Collections.Generic;

namespace IGDB.Scraper {
public static class IGDBCredentials {
    public const string ClientId = "";
    public const string Auth = "";

    public static readonly Dictionary<string, string> HeadersDictionary = new Dictionary<string, string> {
        { "Client-ID", ClientId },
        { "Authorization", Auth }
    };
}
}