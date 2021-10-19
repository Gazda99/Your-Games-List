using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IGDB.Scraper {
class Program {
    static async Task Main(string[] args) {
        DbContextOptionsBuilder<YGL.Model.YGLDataContext> optionsBuilder =
            new DbContextOptionsBuilder<YGL.Model.YGLDataContext>();

        string sqlConnectionString =
            (await File.ReadAllLinesAsync("../../../../Twitch Data.txt"))[2].Replace(@"\\", @"\");

        optionsBuilder.UseSqlServer(sqlConnectionString);

        YGL.Model.YGLDataContext dbConnection = new YGL.Model.YGLDataContext(optionsBuilder.Options);
        await dbConnection.Database.OpenConnectionAsync();

        await Scrape(dbConnection);

        await dbConnection.Database.CloseConnectionAsync();
    }

    private static async Task Scrape(YGL.Model.YGLDataContext dbConnection) {
        // await Scrapers.ScrapCompanies(dbConnection);
        // await Scrapers.ScrapGenres(dbConnection);
        // await Scrapers.ScrapGameModes(dbConnection);
        // await Scrapers.ScrapPlatforms(dbConnection);
        await Scrapers.ScrapPlayerPerspectives(dbConnection);
    }
}
}