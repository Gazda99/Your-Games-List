using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YGLModel = YGL.Model;
using IGDBModel = IGDB.Model;

namespace IGDB.Scraper {
public static class Scrapers {
    private const string Query = "fields *; ";
    private const string IGDBUrl = "https://api.igdb.com/v4/";
    private const int Limit = 500;

    public static async Task ScrapCompanies(YGLModel.YGLDataContext dbConnection) {
        Scraper scraper = new Scraper {
            Url = IGDBUrl + "companies"
        };

        List<IGDBModel.Company> list = await scraper.Scrape<IGDBModel.Company>(Query, Limit);

        foreach (IGDBModel.Company scrapedItem in list) {
            if (scrapedItem.Id is null) continue;

            YGLModel.Company newItem = new YGLModel.Company() {
                Id = (int)scrapedItem.Id,
                Name = scrapedItem.Name ?? String.Empty,
                Description = scrapedItem.Description ?? String.Empty,
                Country = (short?)scrapedItem.Country ?? -1,
                ItemStatus = true
            };

            YGLModel.Company foundItem = await dbConnection.Companies.FirstOrDefaultAsync(i => i.Id == newItem.Id);

            if (foundItem is null)
                await dbConnection.Companies.AddAsync(newItem);
            else
                dbConnection.Entry(foundItem).CurrentValues.SetValues(newItem);
        }

        await dbConnection.SaveChangesAsync();
    }

    public static async Task ScrapGenres(YGLModel.YGLDataContext dbConnection) {
        Scraper scraper = new Scraper {
            Url = IGDBUrl + "genres"
        };

        List<IGDBModel.Genre> list = await scraper.Scrape<IGDBModel.Genre>(Query, Limit);

        foreach (IGDBModel.Genre scrapedItem in list) {
            if (scrapedItem.Id is null) continue;

            YGLModel.Genre newItem = new YGLModel.Genre() {
                Id = (int)scrapedItem.Id,
                Name = scrapedItem.Name ?? String.Empty,
                ItemStatus = true
            };

            YGLModel.Genre foundItem = await dbConnection.Genres.FirstOrDefaultAsync(i => i.Id == newItem.Id);

            if (foundItem is null)
                await dbConnection.Genres.AddAsync(newItem);
            else
                dbConnection.Entry(foundItem).CurrentValues.SetValues(newItem);
        }

        await dbConnection.SaveChangesAsync();
    }

    public static async Task ScrapGameModes(YGLModel.YGLDataContext dbConnection) {
        Scraper scraper = new Scraper {
            Url = IGDBUrl + "game_modes"
        };

        List<IGDBModel.GameMode> list = await scraper.Scrape<IGDBModel.GameMode>(Query, Limit);

        foreach (IGDBModel.GameMode scrapedItem in list) {
            if (scrapedItem.Id is null) continue;

            YGLModel.GameMode newItem = new YGLModel.GameMode() {
                Id = (int)scrapedItem.Id,
                Name = scrapedItem.Name ?? String.Empty,
                ItemStatus = true
            };

            YGLModel.GameMode foundItem = await dbConnection.GameModes.FirstOrDefaultAsync(i => i.Id == newItem.Id);

            if (foundItem is null)
                await dbConnection.GameModes.AddAsync(newItem);
            else
                dbConnection.Entry(foundItem).CurrentValues.SetValues(newItem);
        }

        await dbConnection.SaveChangesAsync();
    }

    public static async Task ScrapPlatforms(YGLModel.YGLDataContext dbConnection) {
        Scraper scraper = new Scraper {
            Url = IGDBUrl + "platforms"
        };

        List<IGDBModel.Platform> list = await scraper.Scrape<IGDBModel.Platform>(Query, Limit);

        foreach (IGDBModel.Platform scrapedItem in list) {
            if (scrapedItem.Id is null) continue;

            YGLModel.Platform newItem = new YGLModel.Platform() {
                Id = (int)scrapedItem.Id,
                Name = scrapedItem.Name ?? String.Empty,
                Abbr = scrapedItem.Abbreviation ?? String.Empty,
                ItemStatus = true
            };

            YGLModel.Platform foundItem = await dbConnection.Platforms.FirstOrDefaultAsync(i => i.Id == newItem.Id);

            if (foundItem is null)
                await dbConnection.Platforms.AddAsync(newItem);
            else
                dbConnection.Entry(foundItem).CurrentValues.SetValues(newItem);
        }

        await dbConnection.SaveChangesAsync();
    }

    public static async Task ScrapPlayerPerspectives(YGLModel.YGLDataContext dbConnection) {
        Scraper scraper = new Scraper {
            Url = IGDBUrl + "player_perspectives"
        };

        List<IGDBModel.PlayerPerspective> list = await scraper.Scrape<IGDBModel.PlayerPerspective>(Query, Limit);

        foreach (IGDBModel.PlayerPerspective scrapedItem in list) {
            if (scrapedItem.Id is null) continue;

            YGLModel.PlayerPerspective newItem = new YGLModel.PlayerPerspective() {
                Id = (int)scrapedItem.Id,
                Name = scrapedItem.Name ?? String.Empty,
                ItemStatus = true
            };

            YGLModel.PlayerPerspective foundItem =
                await dbConnection.PlayerPerspectives.FirstOrDefaultAsync(i => i.Id == newItem.Id);

            if (foundItem is null)
                await dbConnection.PlayerPerspectives.AddAsync(newItem);
            else
                dbConnection.Entry(foundItem).CurrentValues.SetValues(newItem);
        }

        await dbConnection.SaveChangesAsync();
    }
}
}