using System;
using System.Collections.Generic;
using System.Linq;
using YGL.Model;

namespace IGDB.Scraper;

public class EntitiesLists {
    public Dictionary<int, YGL.Model.Game> Games = new Dictionary<int, YGL.Model.Game>();

    public Dictionary<(int, int, bool, bool, bool, bool), YGL.Model.GameHasCompany> GameHasCompanies =
        new Dictionary<(int, int, bool, bool, bool, bool), YGL.Model.GameHasCompany>();

    public Dictionary<(int, int, short), YGL.Model.GameAndPlatformWithReleaseDate> GameAndPlatformWithReleaseDates =
        new Dictionary<(int, int, short), YGL.Model.GameAndPlatformWithReleaseDate>();

    public Dictionary<(int, int), YGL.Model.GameHasGameMode> GameHasGameModes =
        new Dictionary<(int, int), YGL.Model.GameHasGameMode>();

    public Dictionary<(int, int), YGL.Model.GameHasGenre> GameHasGenres =
        new Dictionary<(int, int), YGL.Model.GameHasGenre>();

    public Dictionary<(int, int), YGL.Model.GameHasPlayerPerspective> GameHasPlayerPerspectives =
        new Dictionary<(int, int), YGL.Model.GameHasPlayerPerspective>();

    public Dictionary<(int, byte), YGL.Model.GameWithAgeCategory> GameWithAgeCategories =
        new Dictionary<(int, byte), YGL.Model.GameWithAgeCategory>();

    public Dictionary<(int, int), YGL.Model.GameHasTheme> GameHasThemes =
        new Dictionary<(int, int), YGL.Model.GameHasTheme>();

    public Dictionary<(int, string), YGL.Model.GameWithWebsite> GameWithWebsites =
        new Dictionary<(int, string), YGL.Model.GameWithWebsite>();

    public Dictionary<(int, int), YGL.Model.Dlc> Dlcs = new Dictionary<(int, int), YGL.Model.Dlc>();

    public Dictionary<(int, int), YGL.Model.Expansion> Expansions = new Dictionary<(int, int), YGL.Model.Expansion>();

    public EntitiesLists() { }

    // This surely does not look good, but is much more efficient than using i.e. List, so I stick to that solution.
    public static EntitiesLists PreloadData(YGLDataContext dbConnection) {
        return new EntitiesLists() {
            Games = dbConnection.Games.ToDictionary(g => g.Id, g => g),
            GameHasCompanies = dbConnection.GameHasCompanies.ToDictionary(
                gc => (gc.GameId, gc.CompanyId, gc.IsDeveloper, gc.IsPorting, gc.IsPublisher, gc.IsSupporting),
                gc => gc),
            GameAndPlatformWithReleaseDates =
                dbConnection.GameAndPlatformWithReleaseDates.ToDictionary(
                    gprd => (gprd.GameId, gprd.PlatformId, gprd.Region), gprd => gprd),
            GameHasGenres =
                dbConnection.GameHasGenres.ToDictionary(gg => (gg.GameId, gg.GenreId), gg => gg),
            GameHasGameModes =
                dbConnection.GameHasGameModes.ToDictionary(ggm => (ggm.GameId, ggm.GameModeId),
                    ggm => ggm),
            GameHasPlayerPerspectives =
                dbConnection.GameHasPlayerPerspectives.ToDictionary(
                    gpp => (gpp.GameId, gpp.PlayerPerspectiveId),
                    gpp => gpp),
            GameWithAgeCategories =
                dbConnection.GameWithAgeCategories.ToDictionary(gac => (gac.GameId, gac.Category),
                    gac => gac),
            GameHasThemes =
                dbConnection.GameHasThemes.ToDictionary(
                    gt => (gt.GameId, gt.ThemeId), gt => gt),
            GameWithWebsites =
                dbConnection.GameWithWebsites.ToDictionary(gw =>
                    (gw.GameId, gw.Url), gw => gw)
        };
    }
}