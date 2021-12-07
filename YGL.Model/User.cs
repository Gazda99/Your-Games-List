using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace YGL.Model
{
    [Table("User")]
    public partial class User
    {
        public User()
        {
            CommentGames = new HashSet<CommentGame>();
            EmailConfirmations = new HashSet<EmailConfirmation>();
            FavouriteGames = new HashSet<FavouriteGame>();
            FriendFriendOnes = new HashSet<Friend>();
            FriendFriendTwos = new HashSet<Friend>();
            GroupHasUsers = new HashSet<GroupHasUser>();
            Groups = new HashSet<Group>();
            ListOfGames = new HashSet<ListOfGame>();
            PasswordReset2s = new HashSet<PasswordReset2>();
            PasswordResets = new HashSet<PasswordReset>();
            RefreshTokens = new HashSet<RefreshToken>();
            UserHasBadges = new HashSet<UserHasBadge>();
            UserWithRoles = new HashSet<UserWithRole>();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Username { get; set; }
        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string Email { get; set; }
        [Required]
        [MaxLength(32)]
        public byte[] HashedPassword { get; set; }
        [Required]
        [MaxLength(16)]
        public byte[] Salt { get; set; }
        public byte Gender { get; set; }
        public short BirthYear { get; set; }
        public short Country { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(2000)]
        [Unicode(false)]
        public string About { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Slug { get; set; }
        public short Rank { get; set; }
        public int Experience { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastLogin { get; set; }
        public bool IsEmailConfirmed { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [InverseProperty(nameof(CommentGame.User))]
        public virtual ICollection<CommentGame> CommentGames { get; set; }
        [InverseProperty(nameof(EmailConfirmation.User))]
        public virtual ICollection<EmailConfirmation> EmailConfirmations { get; set; }
        [InverseProperty(nameof(FavouriteGame.User))]
        public virtual ICollection<FavouriteGame> FavouriteGames { get; set; }
        [InverseProperty(nameof(Friend.FriendOne))]
        public virtual ICollection<Friend> FriendFriendOnes { get; set; }
        [InverseProperty(nameof(Friend.FriendTwo))]
        public virtual ICollection<Friend> FriendFriendTwos { get; set; }
        [InverseProperty(nameof(GroupHasUser.User))]
        public virtual ICollection<GroupHasUser> GroupHasUsers { get; set; }
        [InverseProperty(nameof(Group.Creator))]
        public virtual ICollection<Group> Groups { get; set; }
        [InverseProperty(nameof(ListOfGame.Owner))]
        public virtual ICollection<ListOfGame> ListOfGames { get; set; }
        [InverseProperty(nameof(PasswordReset2.User))]
        public virtual ICollection<PasswordReset2> PasswordReset2s { get; set; }
        [InverseProperty(nameof(PasswordReset.User))]
        public virtual ICollection<PasswordReset> PasswordResets { get; set; }
        [InverseProperty(nameof(RefreshToken.User))]
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        [InverseProperty(nameof(UserHasBadge.User))]
        public virtual ICollection<UserHasBadge> UserHasBadges { get; set; }
        [InverseProperty(nameof(UserWithRole.User))]
        public virtual ICollection<UserWithRole> UserWithRoles { get; set; }
    }
}
