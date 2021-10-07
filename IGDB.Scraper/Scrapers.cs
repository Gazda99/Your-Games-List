using System;
using System.Threading.Tasks;
using IGDB.Model;
using Microsoft.EntityFrameworkCore;

namespace IGDB.Scraper {
public static class Scrapers {
    private const string Query = "fields *; ";
    private const string IGDBUrl = "https://api.igdb.com/v4/";
    private const int Limit = 500;

    public static async Task ScrapCompanies(YGL.Model.YGLDataContext dbConnection) {
        Scraper scraper = new Scraper {
            Url = IGDBUrl + "companies"
        };

        var list = await scraper.Scrape<Company>(Query, Limit);

        foreach (Company scrapedItem in list) {
            if (scrapedItem.Id is null) continue;

            YGL.Model.Company newItem = new YGL.Model.Company() {
                Id = (int)scrapedItem.Id,
                Name = scrapedItem.Name ?? String.Empty,
                Description = scrapedItem.Description ?? String.Empty,
                Country = (short?)scrapedItem.Country ?? -1,
                ItemStatus = true
            };

            var check = await dbConnection.Companies.FirstOrDefaultAsync(i => i.Id == newItem.Id);

            if (check is null)
                await dbConnection.Companies.AddAsync(newItem);
            else
                dbConnection.Entry(check).CurrentValues.SetValues(newItem);
        }

        await dbConnection.SaveChangesAsync();
    }
}
}