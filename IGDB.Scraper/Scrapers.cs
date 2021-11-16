using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IGDB.Scraper {
public class Scrapers {
    private const string BasicQuery = "fields *; ";
    private const string IGDBUrl = "https://api.igdb.com/v4/";

    /// <summary>
    /// How many items will be requested in query
    /// </summary>
    public int LimitInOneRequest { get; set; } = 500;

    /// <summary>
    /// How many queries will be executed at one time
    /// </summary>
    public int Lop { get; set; } = 4;

    private readonly YGL.Model.YGLDataContext _dbConnection;

    private List<Tuple<int, int>> _dlcs;
    private List<Tuple<int, int>> _expansions;

    public Scrapers(YGL.Model.YGLDataContext dbConnection) {
        this._dbConnection = dbConnection;
    }

    /// <summary>
    /// Copies the Company objects from IGDB database to YGL database
    /// </summary>
    public async Task ScrapeCompanies() {
        await ScrapeConvertAndAddOrUpdateItem<IGDB.Model.Company>("companies", ConvertAndAddOrUpdateCompany);
    }

    /// <summary>
    /// Copies the GameMode objects from IGDB database to YGL database
    /// </summary>
    public async Task ScrapeGameModes() {
        await ScrapeConvertAndAddOrUpdateItem<IGDB.Model.GameMode>("game_modes", ConvertAndAddOrUpdateGameMode);
    }

    /// <summary>
    /// Copies the Game objects, and all related to them from IGDB database to YGL database
    /// </summary>
    public async Task ScrapeGames() {
        const string customQuery =
            "fields id,category, name, storyline, slug, summary, cover.image_id, involved_companies.company.id, involved_companies.supporting, involved_companies.porting, involved_companies.publisher, involved_companies.developer, release_dates.date, release_dates.platform.id, game_modes, genres, player_perspectives, age_ratings.category, age_ratings.rating, themes, websites.category, websites.url, dlcs, expansions;";

        _dlcs = new List<Tuple<int, int>>();
        _expansions = new List<Tuple<int, int>>();

        await ScrapeConvertAndAddOrUpdateItem<IGDB.Model.Game>("games", null, customQuery);
    }

    /// <summary>
    /// Copies the Genre objects from IGDB database to YGL database
    /// </summary>
    public async Task ScrapeGenres() {
        await ScrapeConvertAndAddOrUpdateItem<IGDB.Model.Genre>("genres", ConvertAndAddOrUpdateGenre);
    }

    /// <summary>
    /// Copies the Platform objects from IGDB database to YGL database
    /// </summary>
    public async Task ScrapePlatforms() {
        await ScrapeConvertAndAddOrUpdateItem<IGDB.Model.Platform>("platforms", ConvertAndAddOrUpdatePlatform);
    }

    /// <summary>
    /// Copies the PlayerPerspective objects from IGDB database to YGL database
    /// </summary>
    public async Task ScrapePlayerPerspectives() {
        await ScrapeConvertAndAddOrUpdateItem<IGDB.Model.PlayerPerspective>(
            "player_perspectives", ConvertAndAddOrUpdatePlayerPerspective);
    }

    /// <summary>
    /// Copies the Theme objects from IGDB database to YGL database
    /// </summary>
    public async Task ScrapeThemes() {
        await ScrapeConvertAndAddOrUpdateItem<IGDB.Model.Theme>("themes", ConvertAndAddOrUpdateTheme);
    }


    private delegate Task ConvertAndAddOrUpdate<in TIGDB>(TIGDB igdbModel, CountersDict countersDict);
    
    /// <summary>
    /// Runs the generic scraper with provided parameters
    /// </summary>
    /// <param name="endpoint">IGDB endpoint</param>
    /// <param name="convertAndAddOrUpdateFunc">Function responsible for converting the downloaded objects, and then adding/updating them into database</param>
    /// <param name="query">Query which will be send to IGDB Api</param>
    /// <typeparam name="TIGDB">Type of objects that will be downloaded</typeparam>
    private async Task ScrapeConvertAndAddOrUpdateItem<TIGDB>(string endpoint,
        ConvertAndAddOrUpdate<TIGDB> convertAndAddOrUpdateFunc, string query = BasicQuery) {
        GenericScraper genericScraper = new GenericScraper() { Url = IGDBUrl + endpoint };

        List<TIGDB> list = await genericScraper.Scrape<TIGDB>(query, Lop, LimitInOneRequest);

        CountersDict countersDict = new CountersDict();

        Stopwatch operationSw = new Stopwatch();
        operationSw.Start();

        int i = 0;
        if (endpoint == "games") {
            // Preloading all needed tabled from YGl to speed up scraping of Games. 
            EntitiesLists preload = EntitiesLists.PreloadData(_dbConnection);
            // Keeps tracking the added and updated objects. This is helpful when some objects coming from IGDB Api are the same as others,
            // to not make duplicates in YGL database.
            EntitiesLists added = new EntitiesLists();
            EntitiesLists updated = new EntitiesLists();

            foreach (TIGDB scrapedItem in list) {
                ConvertAndAddOrUpdateGame(scrapedItem as IGDB.Model.Game, countersDict, preload, added, updated);
                Console.WriteLine(++i);
            }

            await _dbConnection.SaveChangesAsync();

            preload = new EntitiesLists() {
                Games = _dbConnection.Games.ToDictionary(g => g.Id, x => x),
                Dlcs = _dbConnection.Dlcs.ToDictionary(d => new Tuple<int, int>(d.GameBaseId, d.GameDlcId), d => d),
                Expansions = _dbConnection.Expansions.ToDictionary(e => new Tuple<int, int>(e.GameBaseId, e.GameExpansionId), e => e)
            };

            AddOrUpdateDlcOrExpansion(preload);
        }
        else {
            foreach (TIGDB scrapedItem in list) {
                await convertAndAddOrUpdateFunc(scrapedItem, countersDict);
                Console.WriteLine(++i);
            }
        }

        await _dbConnection.SaveChangesAsync();

        TimeSpan timeSpan = TimeSpan.FromMilliseconds(operationSw.ElapsedMilliseconds);
        operationSw.Stop();

        Console.WriteLine(countersDict.Print());
        Console.WriteLine($"Adding/Updating took {timeSpan.Minutes}m:{timeSpan.Seconds}s:{timeSpan.Milliseconds}ms");
    }


    private async Task ConvertAndAddOrUpdateCompany(IGDB.Model.Company scrapedItem, CountersDict countersDict) {
        if (scrapedItem.Id is null) return;

        Counters counters = new Counters();

        YGL.Model.Company newItem = new YGL.Model.Company() {
            Id = (int)scrapedItem.Id,
            Name = scrapedItem.Name ?? String.Empty,
            Description = scrapedItem.Description ?? String.Empty,
            Country = (short?)scrapedItem.Country ?? -1,
            ItemStatus = true
        };

        YGL.Model.Company foundItem = await _dbConnection.Companies.FirstOrDefaultAsync(i => i.Id == newItem.Id);

        if (foundItem is null) {
            _dbConnection.Companies.Add(newItem);
            counters.Adding++;
        }

        else {
            _dbConnection.Entry(foundItem).CurrentValues.SetValues(newItem);
            counters.Updating++;
        }

        countersDict.Add(nameof(YGL.Model.Company), counters);
    }

    private async Task ConvertAndAddOrUpdatePlayerPerspective(IGDB.Model.PlayerPerspective scrapedItem,
        CountersDict countersDict) {
        if (scrapedItem.Id is null) return;

        Counters counters = new Counters();

        YGL.Model.PlayerPerspective newItem = new YGL.Model.PlayerPerspective() {
            Id = (int)scrapedItem.Id,
            Name = scrapedItem.Name ?? String.Empty,
            ItemStatus = true
        };

        YGL.Model.PlayerPerspective foundItem =
            await _dbConnection.PlayerPerspectives.FirstOrDefaultAsync(i => i.Id == newItem.Id);

        if (foundItem is null) {
            _dbConnection.PlayerPerspectives.Add(newItem);
            counters.Adding++;
        }
        else {
            _dbConnection.Entry(foundItem).CurrentValues.SetValues(newItem);
            counters.Updating++;
        }

        countersDict.Add(nameof(YGL.Model.PlayerPerspective), counters);
    }

    private async Task ConvertAndAddOrUpdatePlatform(IGDB.Model.Platform scrapedItem, CountersDict countersDict) {
        if (scrapedItem.Id is null) return;

        Counters counters = new Counters();

        YGL.Model.Platform newItem = new YGL.Model.Platform() {
            Id = (int)scrapedItem.Id,
            Name = scrapedItem.Name ?? String.Empty,
            Abbr = scrapedItem.Abbreviation ?? String.Empty,
            ItemStatus = true
        };

        YGL.Model.Platform foundItem = await _dbConnection.Platforms.FirstOrDefaultAsync(i => i.Id == newItem.Id);

        if (foundItem is null) {
            _dbConnection.Platforms.Add(newItem);
            counters.Adding++;
        }
        else {
            _dbConnection.Entry(foundItem).CurrentValues.SetValues(newItem);
            counters.Updating++;
        }

        countersDict.Add(nameof(YGL.Model.Platform), counters);
    }

    private async Task ConvertAndAddOrUpdateGameMode(IGDB.Model.GameMode scrapedItem, CountersDict countersDict) {
        if (scrapedItem.Id is null) return;

        Counters counters = new Counters();

        YGL.Model.GameMode newItem = new YGL.Model.GameMode() {
            Id = (int)scrapedItem.Id,
            Name = scrapedItem.Name ?? String.Empty,
            ItemStatus = true
        };

        YGL.Model.GameMode foundItem = await _dbConnection.GameModes.FirstOrDefaultAsync(i => i.Id == newItem.Id);

        if (foundItem is null) {
            _dbConnection.GameModes.Add(newItem);
            counters.Adding++;
        }
        else {
            _dbConnection.Entry(foundItem).CurrentValues.SetValues(newItem);
            counters.Updating++;
        }

        countersDict.Add(nameof(YGL.Model.GameMode), counters);
    }

    private async Task ConvertAndAddOrUpdateGenre(IGDB.Model.Genre scrapedItem, CountersDict countersDict) {
        if (scrapedItem.Id is null) return;

        Counters counters = new Counters();

        YGL.Model.Genre newItem = new YGL.Model.Genre() {
            Id = (int)scrapedItem.Id,
            Name = scrapedItem.Name ?? String.Empty,
            ItemStatus = true
        };

        YGL.Model.Genre foundItem = await _dbConnection.Genres.FirstOrDefaultAsync(i => i.Id == newItem.Id);

        if (foundItem is null) {
            _dbConnection.Genres.Add(newItem);
            counters.Adding++;
        }
        else {
            _dbConnection.Entry(foundItem).CurrentValues.SetValues(newItem);
            counters.Updating++;
        }

        countersDict.Add(nameof(YGL.Model.Genre), counters);
    }

    private async Task ConvertAndAddOrUpdateTheme(IGDB.Model.Theme scrapedItem, CountersDict countersDict) {
        if (scrapedItem.Id is null) return;

        Counters counters = new Counters();

        YGL.Model.Theme newItem = new YGL.Model.Theme() {
            Id = (int)scrapedItem.Id,
            Name = scrapedItem.Name ?? String.Empty,
            ItemStatus = true
        };

        YGL.Model.Theme foundItem = await _dbConnection.Themes.FirstOrDefaultAsync(i => i.Id == newItem.Id);

        if (foundItem is null) {
            _dbConnection.Themes.Add(newItem);
            counters.Adding++;
        }

        else {
            _dbConnection.Entry(foundItem).CurrentValues.SetValues(newItem);
            counters.Updating++;
        }

        countersDict.Add(nameof(YGL.Model.Theme), counters);
    }

    private void ConvertAndAddOrUpdateGame(IGDB.Model.Game scrapedItem, CountersDict countersDict,
        EntitiesLists preload, EntitiesLists added, EntitiesLists updated) {
        if (scrapedItem.Id is null) return;

        Counters counters = new Counters();

        YGL.Model.Game newGame = new YGL.Model.Game() {
            Id = (int)scrapedItem.Id,
            Category = (int?)scrapedItem.Category,
            Name = scrapedItem.Name ?? String.Empty,
            Storyline = scrapedItem.Storyline ?? String.Empty,
            Slug = scrapedItem.Slug ?? String.Empty,
            Summary = scrapedItem.Summary ?? String.Empty,
            ImageId = String.Empty,
            ItemStatus = true
        };

        if (scrapedItem.Cover?.Value != null)
            newGame.ImageId = scrapedItem.Cover.Value.ImageId ?? String.Empty;

        YGL.Model.Game foundGame = preload.Games.GetValueOrDefault(newGame.Id);
        if (foundGame is null) {
            _dbConnection.Games.Add(newGame);
            counters.Adding++;
        }
        else {
            foundGame.Category = newGame.Category;
            foundGame.Name = newGame.Name;
            foundGame.Storyline = newGame.Storyline;
            foundGame.Slug = newGame.Slug;
            foundGame.Summary = newGame.Summary;
            foundGame.ImageId = newGame.ImageId;
            counters.Updating++;
        }

        countersDict.Add(nameof(YGL.Model.Game), counters);
        counters = new Counters();

        #region Companies

        if (scrapedItem.InvolvedCompanies?.Values != null) {
            foreach (IGDB.Model.InvolvedCompany scrapedInvolvedCompany in scrapedItem.InvolvedCompanies.Values) {
                if (scrapedInvolvedCompany.Id == null || scrapedInvolvedCompany.Company.Value.Id == null) continue;

                YGL.Model.GameHasCompany newGameHasCompany = new YGL.Model.GameHasCompany {
                    CompanyId = (int)scrapedInvolvedCompany.Company.Value.Id,
                    GameId = newGame.Id,
                    IsDeveloper = scrapedInvolvedCompany.Developer ?? false,
                    IsPorting = scrapedInvolvedCompany.Porting ?? false,
                    IsPublisher = scrapedInvolvedCompany.Publisher ?? false,
                    IsSupporting = scrapedInvolvedCompany.Supporting ?? false,
                    ItemStatus = true
                };

                Tuple<int, int, bool, bool, bool, bool> tuple = new Tuple<int, int, bool, bool, bool, bool>(
                    newGame.Id, newGameHasCompany.CompanyId,
                    newGameHasCompany.IsDeveloper, newGameHasCompany.IsPorting, newGameHasCompany.IsPublisher,
                    newGameHasCompany.IsSupporting);

                YGL.Model.GameHasCompany foundGameHasCompany = preload.GameHasCompanies.GetValueOrDefault(tuple);

                if (foundGameHasCompany is null) {
                    if (added.GameHasCompanies.ContainsKey(tuple)) continue;

                    _dbConnection.GameHasCompanies.Add(newGameHasCompany);
                    added.GameHasCompanies.Add(tuple, newGameHasCompany);
                    counters.Adding++;
                }
                else {
                    foundGameHasCompany.CompanyId = newGameHasCompany.CompanyId;
                    foundGameHasCompany.GameId = newGameHasCompany.GameId;
                    foundGameHasCompany.IsDeveloper = newGameHasCompany.IsDeveloper;
                    foundGameHasCompany.IsPorting = newGameHasCompany.IsPorting;
                    foundGameHasCompany.IsPublisher = newGameHasCompany.IsPublisher;
                    foundGameHasCompany.IsSupporting = newGameHasCompany.IsSupporting;
                    counters.Updating++;
                }
            }
        }

        countersDict.Add(nameof(YGL.Model.GameHasCompany), counters);
        counters = new Counters();

        #endregion

        #region ReleaseDates

        if (scrapedItem.ReleaseDates?.Values != null) {
            foreach (IGDB.Model.ReleaseDate scrapedReleaseDate in scrapedItem.ReleaseDates.Values) {
                if (scrapedReleaseDate.Id == null || scrapedReleaseDate.Platform?.Value?.Id == null) continue;

                YGL.Model.GameAndPlatformWithReleaseDate newReleaseDate = new YGL.Model.GameAndPlatformWithReleaseDate() {
                    PlatformId = (int)scrapedReleaseDate.Platform.Value.Id,
                    GameId = newGame.Id,
                    ReleaseDate = scrapedReleaseDate.Date?.Date ?? DateTime.UnixEpoch,
                    Region = (short?)scrapedReleaseDate.Region ?? -1,
                    ItemStatus = true
                };

                Tuple<int, int, int> tuple = new Tuple<int, int, int>(newGame.Id, newReleaseDate.PlatformId, newReleaseDate.Region);
                YGL.Model.GameAndPlatformWithReleaseDate
                    foundReleaseDate = preload.GameAndPlatformWithReleaseDates.GetValueOrDefault(tuple);

                if (foundReleaseDate is null) {
                    if (added.GameAndPlatformWithReleaseDates.ContainsKey(tuple)) continue;

                    _dbConnection.GameAndPlatformWithReleaseDates.Add(newReleaseDate);
                    added.GameAndPlatformWithReleaseDates.Add(tuple, newReleaseDate);
                    counters.Adding++;
                }
                else {
                    foundReleaseDate.PlatformId = newReleaseDate.PlatformId;
                    foundReleaseDate.GameId = newReleaseDate.GameId;
                    foundReleaseDate.ReleaseDate = newReleaseDate.ReleaseDate;
                    foundReleaseDate.Region = newReleaseDate.Region;
                    foundReleaseDate.ItemStatus = newReleaseDate.ItemStatus;
                    counters.Updating++;
                }
            }
        }

        countersDict.Add(nameof(YGL.Model.GameAndPlatformWithReleaseDate), counters);
        counters = new Counters();

        #endregion

        #region GameModes

        if (scrapedItem.GameModes?.Ids != null) {
            foreach (long scrapedGameModeId in scrapedItem.GameModes.Ids) {
                YGL.Model.GameHasGameMode newGameHasGameMode = new YGL.Model.GameHasGameMode() {
                    GameId = newGame.Id,
                    GameModeId = (int)scrapedGameModeId,
                    ItemStatus = true
                };

                Tuple<int, int> tuple = new Tuple<int, int>(newGame.Id, newGameHasGameMode.GameModeId);
                YGL.Model.GameHasGameMode foundGameMode = preload.GameHasGameModes.GetValueOrDefault(tuple);

                if (foundGameMode is null) {
                    if (added.GameHasGameModes.ContainsKey(tuple)) continue;

                    _dbConnection.GameHasGameModes.Add(newGameHasGameMode);
                    added.GameHasGameModes.Add(tuple, newGameHasGameMode);
                    counters.Adding++;
                }
                else {
                    foundGameMode.GameId = newGameHasGameMode.GameId;
                    foundGameMode.GameModeId = newGameHasGameMode.GameModeId;
                    counters.Updating++;
                }
            }
        }

        countersDict.Add(nameof(YGL.Model.GameHasGameMode), counters);
        counters = new Counters();

        #endregion

        #region Genres

        if (scrapedItem.Genres?.Ids != null) {
            foreach (long scrapedGenresId in scrapedItem.Genres.Ids) {
                YGL.Model.GameHasGenre newGameHasGenre = new YGL.Model.GameHasGenre() {
                    GameId = newGame.Id,
                    GenreId = (int)scrapedGenresId,
                    ItemStatus = true
                };


                Tuple<int, int> tuple = new Tuple<int, int>(newGame.Id, newGameHasGenre.GenreId);
                YGL.Model.GameHasGenre foundGenre = preload.GameHasGenres.GetValueOrDefault(tuple);

                if (foundGenre is null) {
                    if (added.GameHasGenres.ContainsKey(tuple)) continue;

                    _dbConnection.GameHasGenres.Add(newGameHasGenre);
                    added.GameHasGenres.Add(tuple, newGameHasGenre);
                    counters.Adding++;
                }
                else {
                    foundGenre.GameId = newGameHasGenre.GameId;
                    foundGenre.GenreId = newGameHasGenre.GenreId;
                    counters.Updating++;
                }
            }
        }

        countersDict.Add(nameof(YGL.Model.GameHasGenre), counters);
        counters = new Counters();

        #endregion

        #region PlayerPerspectives

        if (scrapedItem.PlayerPerspectives?.Ids != null) {
            foreach (long scrapedPerspectiveId in scrapedItem.PlayerPerspectives.Ids) {
                YGL.Model.GameHasPlayerPerspective newGameHasPerspective = new YGL.Model.GameHasPlayerPerspective() {
                    GameId = newGame.Id,
                    PlayerPerspectiveId = (int)scrapedPerspectiveId,
                    ItemStatus = true
                };

                Tuple<int, int> tuple = new Tuple<int, int>(newGame.Id, newGameHasPerspective.PlayerPerspectiveId);
                YGL.Model.GameHasPlayerPerspective foundPerspective = preload.GameHasPlayerPerspectives.GetValueOrDefault(tuple);

                if (foundPerspective is null) {
                    if (added.GameHasPlayerPerspectives.ContainsKey(tuple)) continue;

                    _dbConnection.GameHasPlayerPerspectives.Add(newGameHasPerspective);
                    added.GameHasPlayerPerspectives.Add(tuple, newGameHasPerspective);
                    counters.Adding++;
                }
                else {
                    foundPerspective.GameId = newGameHasPerspective.GameId;
                    foundPerspective.PlayerPerspectiveId = newGameHasPerspective.PlayerPerspectiveId;
                    counters.Updating++;
                }
            }
        }

        countersDict.Add(nameof(YGL.Model.GameHasPlayerPerspective), counters);
        counters = new Counters();

        #endregion

        #region AgeCategories

        if (scrapedItem.AgeRatings?.Values != null) {
            foreach (IGDB.Model.AgeRating scrapedAgeRatings in scrapedItem.AgeRatings.Values) {
                YGL.Model.GameWithAgeCategory newGameWithAgeCategory = new YGL.Model.GameWithAgeCategory() {
                    GameId = newGame.Id,
                    Category = (byte)scrapedAgeRatings.Category,
                    Rating = (byte)scrapedAgeRatings.Rating,
                    ItemStatus = true
                };

                Tuple<int, byte> tuple = new Tuple<int, byte>(newGame.Id, newGameWithAgeCategory.Category);
                YGL.Model.GameWithAgeCategory foundAgeRating = preload.GameWithAgeCategories.GetValueOrDefault(tuple);
                if (foundAgeRating is null) {
                    if (added.GameWithAgeCategories.ContainsKey(tuple)) continue;

                    _dbConnection.GameWithAgeCategories.Add(newGameWithAgeCategory);
                    added.GameWithAgeCategories.Add(tuple, newGameWithAgeCategory);
                    counters.Adding++;
                }
                else {
                    foundAgeRating.GameId = newGameWithAgeCategory.GameId;
                    foundAgeRating.Category = newGameWithAgeCategory.Category;
                    foundAgeRating.Rating = newGameWithAgeCategory.Rating;
                    counters.Updating++;
                }
            }
        }

        countersDict.Add(nameof(YGL.Model.GameWithAgeCategory), counters);
        counters = new Counters();

        #endregion

        #region Themes

        if (scrapedItem.Themes?.Ids != null) {
            foreach (long scrapedThemeId in scrapedItem.Themes.Ids) {
                YGL.Model.GameHasTheme newGameHasTheme = new YGL.Model.GameHasTheme() {
                    GameId = newGame.Id,
                    ThemeId = (int)scrapedThemeId,
                    ItemStatus = true
                };


                Tuple<int, int> tuple = new Tuple<int, int>(newGame.Id, newGameHasTheme.ThemeId);
                YGL.Model.GameHasTheme foundGameHasThemes = preload.GameHasThemes.GetValueOrDefault(tuple);
                if (foundGameHasThemes is null) {
                    if (added.GameHasThemes.ContainsKey(tuple)) continue;

                    _dbConnection.GameHasThemes.Add(newGameHasTheme);
                    added.GameHasThemes.Add(tuple, newGameHasTheme);
                    counters.Adding++;
                }
                else {
                    foundGameHasThemes.GameId = newGameHasTheme.GameId;
                    foundGameHasThemes.ThemeId = newGameHasTheme.ThemeId;
                    counters.Updating++;
                }
            }
        }


        countersDict.Add(nameof(YGL.Model.GameHasTheme), counters);
        counters = new Counters();

        #endregion

        #region Websites

        if (scrapedItem.Websites?.Values != null) {
            foreach (IGDB.Model.Website scrapedWebsite in scrapedItem.Websites.Values) {
                YGL.Model.GameWithWebsite newGameWithWebsite = new YGL.Model.GameWithWebsite() {
                    GameId = newGame.Id,
                    Category = (byte)scrapedWebsite.Category,
                    Url = scrapedWebsite.Url ?? String.Empty,
                    ItemStatus = true
                };

                Tuple<int, string> tuple = new Tuple<int, string>(newGame.Id, newGameWithWebsite.Url);
                YGL.Model.GameWithWebsite foundWebsite = preload.GameWithWebsites.GetValueOrDefault(tuple);

                if (foundWebsite is null) {
                    if (added.GameWithWebsites.ContainsKey(tuple)) continue;

                    _dbConnection.GameWithWebsites.Add(newGameWithWebsite);
                    added.GameWithWebsites.Add(tuple, newGameWithWebsite);
                    counters.Adding++;
                }
                else {
                    foundWebsite.GameId = newGameWithWebsite.GameId;
                    foundWebsite.Category = newGameWithWebsite.Category;
                    foundWebsite.Url = newGameWithWebsite.Url;

                    counters.Updating++;
                }
            }
        }

        countersDict.Add(nameof(YGL.Model.GameWithWebsite), counters);

        #endregion

        //dlcs
        if (scrapedItem.Dlcs?.Ids != null) {
            foreach (int dlcId in scrapedItem.Dlcs.Ids.Select(i => (int)i)) {
                if (_dlcs.Any(x => x.Item1 == newGame.Id && x.Item2 == dlcId))
                    continue;
                _dlcs.Add(new Tuple<int, int>(newGame.Id, dlcId));
            }
        }

        //expansions
        if (scrapedItem.Expansions?.Ids != null) {
            foreach (int expansionId in scrapedItem.Expansions.Ids.Select(i => (int)i)) {
                if (_expansions.Any(x => x.Item1 == newGame.Id && x.Item2 == expansionId))
                    continue;
                _expansions.Add(new Tuple<int, int>(newGame.Id, expansionId));
            }
        }
    }

    // Working a little bit different than above ones. Does not need to be passed to ScrapeConvertAndAddOrUpdateItem method. It is runned
    // along with ScrapeGames function.
    private void AddOrUpdateDlcOrExpansion(EntitiesLists preload) {
        Counters counters = new Counters();
        CountersDict countersDict = new CountersDict();

        //dlcs
        foreach ((int baseId, int dlcId) in _dlcs) {
            YGL.Model.Dlc newDlc = new YGL.Model.Dlc {
                GameBaseId = baseId,
                GameDlcId = dlcId,
                ItemStatus = true
            };

            YGL.Model.Game foundBaseGame = preload.Games.GetValueOrDefault(baseId);
            YGL.Model.Game foundDlcGame = preload.Games.GetValueOrDefault(dlcId);

            if (foundBaseGame is null) {
                Console.WriteLine($"Base game with id: {baseId} not found in db, cannot add dlc with id: {dlcId}");
                continue;
            }

            if (foundDlcGame is null) {
                Console.WriteLine(
                    $"Dlc game with id: {dlcId} not found in db, cannot add dlc to a game with base id: {baseId}");
                continue;
            }

            YGL.Model.Dlc foundDlc = preload.Dlcs.GetValueOrDefault(new Tuple<int, int>(newDlc.GameBaseId, newDlc.GameDlcId));

            if (foundDlc is null) {
                _dbConnection.Dlcs.Add(newDlc);
                counters.Adding++;
            }
            else {
                foundDlc.GameBaseId = newDlc.GameBaseId;
                foundDlc.GameDlcId = newDlc.GameDlcId;
                counters.Updating++;
            }
        }

        countersDict.Add(nameof(YGL.Model.Dlc), counters);
        counters = new Counters();

        foreach ((int baseId, int expansionId) in _expansions) {
            YGL.Model.Expansion newExpansion = new YGL.Model.Expansion {
                GameBaseId = baseId,
                GameExpansionId = expansionId,
                ItemStatus = true
            };

            YGL.Model.Game foundBaseGame = preload.Games.GetValueOrDefault(baseId);
            YGL.Model.Game foundExpansionGame = preload.Games.GetValueOrDefault(expansionId);

            if (foundBaseGame is null) {
                Console.WriteLine(
                    $"Base game with id: {baseId} not found in db, cannot add expansion with id: {expansionId}");
                continue;
            }

            if (foundExpansionGame is null) {
                Console.WriteLine(
                    $"Expansion game with id: {expansionId} not found in db, cannot add expansion to a game with base id: {baseId}");
                continue;
            }

            YGL.Model.Expansion foundExpansion =
                preload.Expansions.GetValueOrDefault(new Tuple<int, int>(newExpansion.GameBaseId, newExpansion.GameExpansionId));

            if (foundExpansion is null) {
                _dbConnection.Expansions.Add(newExpansion);
                counters.Adding++;
            }
            else {
                foundExpansion.GameBaseId = newExpansion.GameBaseId;
                foundExpansion.GameExpansionId = newExpansion.GameExpansionId;
                counters.Updating++;
            }
        }

        countersDict.Add(nameof(YGL.Model.Expansion), counters);
        Console.WriteLine(countersDict.Print());

        _dlcs.Clear();
        _expansions.Clear();
    }

    //Counters and CountersDict used to count the updated and added items to YGL database 
    private class Counters {
        public int Adding { get; set; }
        public int Updating { get; set; }
    }

    private class CountersDict {
        private readonly Dictionary<string, Counters> _dict = new Dictionary<string, Counters>();

        public void Add(string key, Counters value) {
            if (!_dict.ContainsKey(key))
                _dict.Add(key, value);
            else {
                _dict[key].Adding += value.Adding;
                _dict[key].Updating += value.Updating;
            }
        }

        public void Add(string key, int adding, int updating) {
            Add(key, new Counters() { Adding = adding, Updating = updating });
        }

        public string Print() {
            StringBuilder sb = new StringBuilder();

            foreach ((string key, Counters value) in _dict)
                sb.Append($"Added {value.Adding} and updated {value.Updating} of {key} objects to database\n");

            return sb.ToString();
        }
    }
}
}