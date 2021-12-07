using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace YGL.Model
{
    [Table("Game")]
    public partial class Game
    {
        public Game()
        {
            CommentGames = new HashSet<CommentGame>();
            DlcGameBases = new HashSet<Dlc>();
            DlcGameDlcs = new HashSet<Dlc>();
            ExpansionGameBases = new HashSet<Expansion>();
            ExpansionGameExpansions = new HashSet<Expansion>();
            FavouriteGames = new HashSet<FavouriteGame>();
            GameAndPlatformWithReleaseDates = new HashSet<GameAndPlatformWithReleaseDate>();
            GameHasCompanies = new HashSet<GameHasCompany>();
            GameHasGameModes = new HashSet<GameHasGameMode>();
            GameHasGenres = new HashSet<GameHasGenre>();
            GameHasPlayerPerspectives = new HashSet<GameHasPlayerPerspective>();
            GameHasThemes = new HashSet<GameHasTheme>();
            GameWithAgeCategories = new HashSet<GameWithAgeCategory>();
            GameWithWebsites = new HashSet<GameWithWebsite>();
            ListEntries = new HashSet<ListEntry>();
        }

        [Key]
        public int Id { get; set; }
        public int? Category { get; set; }
        [Required]
        [Unicode(false)]
        public string ImageId { get; set; }
        [Required]
        [Unicode(false)]
        public string Name { get; set; }
        [Required]
        [Unicode(false)]
        public string Storyline { get; set; }
        [Required]
        [Unicode(false)]
        public string Slug { get; set; }
        [Required]
        [Unicode(false)]
        public string Summary { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [InverseProperty(nameof(CommentGame.Game))]
        public virtual ICollection<CommentGame> CommentGames { get; set; }
        [InverseProperty(nameof(Dlc.GameBase))]
        public virtual ICollection<Dlc> DlcGameBases { get; set; }
        [InverseProperty(nameof(Dlc.GameDlc))]
        public virtual ICollection<Dlc> DlcGameDlcs { get; set; }
        [InverseProperty(nameof(Expansion.GameBase))]
        public virtual ICollection<Expansion> ExpansionGameBases { get; set; }
        [InverseProperty(nameof(Expansion.GameExpansion))]
        public virtual ICollection<Expansion> ExpansionGameExpansions { get; set; }
        [InverseProperty(nameof(FavouriteGame.Game))]
        public virtual ICollection<FavouriteGame> FavouriteGames { get; set; }
        [InverseProperty(nameof(GameAndPlatformWithReleaseDate.Game))]
        public virtual ICollection<GameAndPlatformWithReleaseDate> GameAndPlatformWithReleaseDates { get; set; }
        [InverseProperty(nameof(GameHasCompany.Game))]
        public virtual ICollection<GameHasCompany> GameHasCompanies { get; set; }
        [InverseProperty(nameof(GameHasGameMode.Game))]
        public virtual ICollection<GameHasGameMode> GameHasGameModes { get; set; }
        [InverseProperty(nameof(GameHasGenre.Game))]
        public virtual ICollection<GameHasGenre> GameHasGenres { get; set; }
        [InverseProperty(nameof(GameHasPlayerPerspective.Game))]
        public virtual ICollection<GameHasPlayerPerspective> GameHasPlayerPerspectives { get; set; }
        [InverseProperty(nameof(GameHasTheme.Game))]
        public virtual ICollection<GameHasTheme> GameHasThemes { get; set; }
        [InverseProperty(nameof(GameWithAgeCategory.Game))]
        public virtual ICollection<GameWithAgeCategory> GameWithAgeCategories { get; set; }
        [InverseProperty(nameof(GameWithWebsite.Game))]
        public virtual ICollection<GameWithWebsite> GameWithWebsites { get; set; }
        [InverseProperty(nameof(ListEntry.Game))]
        public virtual ICollection<ListEntry> ListEntries { get; set; }
    }
}
