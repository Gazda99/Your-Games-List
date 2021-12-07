using System.Collections.Generic;

namespace IGDB.Scraper; 

public class ScrapeResult<T> {
    public IEnumerable<T> ScrapedItems { get; set; }
    public bool IsSuccess { get; set; } = true;
}