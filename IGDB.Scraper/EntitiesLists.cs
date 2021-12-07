using System;
using System.Collections.Generic;
using System.Linq;
using YGL.Model;

namespace IGDB.Scraper;

public class EntitiesLists {
    public Dictionary<int, YGL.Model.Game> Games = new Dictionary<int, YGL.Model.Game>();

    public Dictionary<Tuple<int, int, bool, bool, bool, bool>, YGL.Model.GameHasCompany> GameHasCompanies =
        new Dictionary<Tuple<int, int, bool, bool, bool, bool>, YGL.Model.GameHasCompany>();

    public Dictionary<Tuple<int, int, int>, YGL.Model.GameAndPlatformWithReleaseDate> GameAndPlatformWithReleaseDates =
        new Dictionary<Tuple<int, int, int>, YGL.Model.GameAndPlatformWithReleaseDate>();

    public Dictionary<Tuple<int, int>, YGL.Model.GameHasGameMode> GameHasGameModes =
        new Dictionary<Tuple<int, int>, YGL.Model.GameHasGameMode>();

    public Dictionary<Tuple<int, int>, YGL.Model.GameHasGenre> GameHasGenres = new Dictionary<Tuple<int, int>, YGL.Model.GameHasGenre>();

    public Dictionary<Tuple<int, int>, YGL.Model.GameHasPlayerPerspective> GameHasPlayerPerspectives =
        new Dictionary<Tuple<int, int>, YGL.Model.GameHasPlayerPerspective>();

    public Dictionary<Tuple<int, byte>, YGL.Model.GameWithAgeCategory> GameWithAgeCategories =
        new Dictionary<Tuple<int, byte>, YGL.Model.GameWithAgeCategory>();

    public Dictionary<Tuple<int, int>, YGL.Model.GameHasTheme> GameHasThemes = new Dictionary<Tuple<int, int>, YGL.Model.GameHasTheme>();

    public Dictionary<Tuple<int, string>, YGL.Model.GameWithWebsite> GameWithWebsites =
        new Dictionary<Tuple<int, string>, YGL.Model.GameWithWebsite>();

    public Dictionary<Tuple<int, int>, YGL.Model.Dlc> Dlcs = new Dictionary<Tuple<int, int>, YGL.Model.Dlc>();
    public Dictionary<Tuple<int, int>, YGL.Model.Expansion> Expansions = new Dictionary<Tuple<int, int>, YGL.Model.Expansion>();

    public EntitiesLists() { }

    // This surely does not look good, but is much more efficient than using i.e. List, so I stick to that solution.
    public static EntitiesLists PreloadData(YGLDataContext dbConnection) {
        return new EntitiesLists() {
            Games = dbConnection.Games.ToDictionary(g => g.Id, g => g),
            GameHasCompanies = dbConnection.GameHasCompanies.ToDictionary(
                gc =>
                    new Tuple<int, int, bool, bool, bool, bool>(gc.GameId, gc.CompanyId,
                        gc.IsDeveloper, gc.IsPorting, gc.IsPublisher, gc.IsSupporting),
                gc => gc),
            GameAndPlatformWithReleaseDates =
                dbConnection.GameAndPlatformWithReleaseDates.ToDictionary(
                    gprd => new Tuple<int, int, int>(gprd.GameId, gprd.PlatformId, gprd.Region), gprd => gprd),
            GameHasGenres = dbConnection.GameHasGenres.ToDictionary(gg => new Tuple<int, int>(gg.GameId, gg.GenreId), gg => gg),
            GameHasGameModes =
                dbConnection.GameHasGameModes.ToDictionary(ggm => new Tuple<int, int>(ggm.GameId, ggm.GameModeId), ggm => ggm),
            GameHasPlayerPerspectives =
                dbConnection.GameHasPlayerPerspectives.ToDictionary(gpp => new Tuple<int, int>(gpp.GameId, gpp.PlayerPerspectiveId),
                    gpp => gpp),
            GameWithAgeCategories =
                dbConnection.GameWithAgeCategories.ToDictionary(gac => new Tuple<int, byte>(gac.GameId, gac.Category), gac => gac),
            GameHasThemes = dbConnection.GameHasThemes.ToDictionary(gt => new Tuple<int, int>(gt.GameId, gt.ThemeId), gt => gt),
            GameWithWebsites = dbConnection.GameWithWebsites.ToDictionary(gw => new Tuple<int, string>(gw.GameId, gw.Url), gw => gw)
        };
    }
}