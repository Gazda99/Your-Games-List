using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace YGL.Model
{
    public partial class YGLDataContext : DbContext
    {
        public YGLDataContext()
        {
        }

        public YGLDataContext(DbContextOptions<YGLDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Badge> Badges { get; set; }
        public virtual DbSet<CommentGame> CommentGames { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Dlc> Dlcs { get; set; }
        public virtual DbSet<EmailConfirmation> EmailConfirmations { get; set; }
        public virtual DbSet<Expansion> Expansions { get; set; }
        public virtual DbSet<FavouriteGame> FavouriteGames { get; set; }
        public virtual DbSet<Friend> Friends { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<GameAndPlatformWithReleaseDate> GameAndPlatformWithReleaseDates { get; set; }
        public virtual DbSet<GameHasCompany> GameHasCompanies { get; set; }
        public virtual DbSet<GameHasGameMode> GameHasGameModes { get; set; }
        public virtual DbSet<GameHasGenre> GameHasGenres { get; set; }
        public virtual DbSet<GameHasPlayerPerspective> GameHasPlayerPerspectives { get; set; }
        public virtual DbSet<GameHasTheme> GameHasThemes { get; set; }
        public virtual DbSet<GameMode> GameModes { get; set; }
        public virtual DbSet<GameWithAgeCategory> GameWithAgeCategories { get; set; }
        public virtual DbSet<GameWithWebsite> GameWithWebsites { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupHasUser> GroupHasUsers { get; set; }
        public virtual DbSet<ListEntry> ListEntries { get; set; }
        public virtual DbSet<ListOfGame> ListOfGames { get; set; }
        public virtual DbSet<PasswordReset> PasswordResets { get; set; }
        public virtual DbSet<PasswordReset2> PasswordReset2s { get; set; }
        public virtual DbSet<Platform> Platforms { get; set; }
        public virtual DbSet<PlayerPerspective> PlayerPerspectives { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Theme> Themes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserHasBadge> UserHasBadges { get; set; }
        public virtual DbSet<UserWithRole> UserWithRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_100_CI_AS_SC_UTF8");

            modelBuilder.Entity<Badge>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<CommentGame>(entity =>
            {
                entity.Property(e => e.Comment).IsUnicode(false);

                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.CommentGames)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommentGame_Game");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CommentGames)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommentGame_User");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Country).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<Dlc>(entity =>
            {
                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.GameBase)
                    .WithMany(p => p.DlcGameBases)
                    .HasForeignKey(d => d.GameBaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dlc_Game__Base");

                entity.HasOne(d => d.GameDlc)
                    .WithMany(p => p.DlcGameDlcs)
                    .HasForeignKey(d => d.GameDlcId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dlc_Game__Dlc");
            });

            modelBuilder.Entity<EmailConfirmation>(entity =>
            {
                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.Url).IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EmailConfirmations)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_EmailConfirmation_User");
            });

            modelBuilder.Entity<Expansion>(entity =>
            {
                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.GameBase)
                    .WithMany(p => p.ExpansionGameBases)
                    .HasForeignKey(d => d.GameBaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Expansion_Game__Base");

                entity.HasOne(d => d.GameExpansion)
                    .WithMany(p => p.ExpansionGameExpansions)
                    .HasForeignKey(d => d.GameExpansionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Expansion_Game__Expansion");
            });

            modelBuilder.Entity<FavouriteGame>(entity =>
            {
                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.FavouriteGames)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK_FavouriteGame_Game");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FavouriteGames)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_FavouriteGame_User");
            });

            modelBuilder.Entity<Friend>(entity =>
            {
                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.FriendOne)
                    .WithMany(p => p.FriendFriendOnes)
                    .HasForeignKey(d => d.FriendOneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Friend_User__FriendOne");

                entity.HasOne(d => d.FriendTwo)
                    .WithMany(p => p.FriendFriendTwos)
                    .HasForeignKey(d => d.FriendTwoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Friend_User__FriendTwo");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ImageId).IsUnicode(false);

                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Slug).IsUnicode(false);

                entity.Property(e => e.Storyline).IsUnicode(false);

                entity.Property(e => e.Summary).IsUnicode(false);
            });

            modelBuilder.Entity<GameAndPlatformWithReleaseDate>(entity =>
            {
                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.Region).HasDefaultValueSql("((-1))");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameAndPlatformWithReleaseDates)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK_GameAndPlatformWithReleaseDate_Game");

                entity.HasOne(d => d.Platform)
                    .WithMany(p => p.GameAndPlatformWithReleaseDates)
                    .HasForeignKey(d => d.PlatformId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GameAndPlatformWithReleaseDate_Platform");
            });

            modelBuilder.Entity<GameHasCompany>(entity =>
            {
                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.GameHasCompanies)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_GameHasCompany_Company");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameHasCompanies)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK_GameHasCompany_Game");
            });

            modelBuilder.Entity<GameHasGameMode>(entity =>
            {
                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameHasGameModes)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK_GameHasGameMode_Game");

                entity.HasOne(d => d.GameMode)
                    .WithMany(p => p.GameHasGameModes)
                    .HasForeignKey(d => d.GameModeId)
                    .HasConstraintName("FK_GameHasGameMode_GameMode");
            });

            modelBuilder.Entity<GameHasGenre>(entity =>
            {
                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameHasGenres)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK_GameHasGenre_Game");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.GameHasGenres)
                    .HasForeignKey(d => d.GenreId)
                    .HasConstraintName("FK_GameHasGenre_Genre");
            });

            modelBuilder.Entity<GameHasPlayerPerspective>(entity =>
            {
                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameHasPlayerPerspectives)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK_GameHasPlayerPerspective_Game");

                entity.HasOne(d => d.PlayerPerspective)
                    .WithMany(p => p.GameHasPlayerPerspectives)
                    .HasForeignKey(d => d.PlayerPerspectiveId)
                    .HasConstraintName("FK_GameHasPlayerPerspective_PlayerPerspective");
            });

            modelBuilder.Entity<GameHasTheme>(entity =>
            {
                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameHasThemes)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK_GameHasTheme_Game");

                entity.HasOne(d => d.Theme)
                    .WithMany(p => p.GameHasThemes)
                    .HasForeignKey(d => d.ThemeId)
                    .HasConstraintName("FK_GameHasTheme_Theme");
            });

            modelBuilder.Entity<GameMode>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<GameWithAgeCategory>(entity =>
            {
                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameWithAgeCategories)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK_GameWithAgeCategory_Game");
            });

            modelBuilder.Entity<GameWithWebsite>(entity =>
            {
                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.Url).IsUnicode(false);

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameWithWebsites)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK_GameWithWebsite_Game");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Slug).IsUnicode(false);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Group_User");
            });

            modelBuilder.Entity<GroupHasUser>(entity =>
            {
                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupHasUsers)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupHasUser_Group");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GroupHasUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupHasUser_User");
            });

            modelBuilder.Entity<ListEntry>(entity =>
            {
                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.ListEntries)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ListEntry_Game");

                entity.HasOne(d => d.List)
                    .WithMany(p => p.ListEntries)
                    .HasForeignKey(d => d.ListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ListEntry_ListOfGames");

                entity.HasOne(d => d.Platform)
                    .WithMany(p => p.ListEntries)
                    .HasForeignKey(d => d.PlatformId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ListEntry_Platform");
            });

            modelBuilder.Entity<ListOfGame>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.ListOfGames)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ListOfGames_User");
            });

            modelBuilder.Entity<PasswordReset>(entity =>
            {
                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.Url).IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PasswordResets)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_PasswordReset_User");
            });

            modelBuilder.Entity<PasswordReset2>(entity =>
            {
                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.Token).IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PasswordReset2s)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_PasswordReset2_User");
            });

            modelBuilder.Entity<Platform>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Abbr).IsUnicode(false);

                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<PlayerPerspective>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.JwtId).IsUnicode(false);

                entity.Property(e => e.Token).IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RefreshTokens)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_RefreshToken_User");
            });

            modelBuilder.Entity<Theme>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.About).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.HashedPassword).IsFixedLength(true);

                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.Salt).IsFixedLength(true);

                entity.Property(e => e.Slug).IsUnicode(false);

                entity.Property(e => e.Username).IsUnicode(false);
            });

            modelBuilder.Entity<UserHasBadge>(entity =>
            {
                entity.Property(e => e.ItemStatus).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Badge)
                    .WithMany(p => p.UserHasBadges)
                    .HasForeignKey(d => d.BadgeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserHasBadge_Badge");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserHasBadges)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserHasBadge_User");
            });

            modelBuilder.Entity<UserWithRole>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserWithRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserWithRole_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
