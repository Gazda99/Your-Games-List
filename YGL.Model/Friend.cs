using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace YGL.Model
{
    [Table("Friend")]
    public partial class Friend
    {
        [Key]
        public long Id { get; set; }
        public long FriendOneId { get; set; }
        public long FriendTwoId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Since { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [ForeignKey(nameof(FriendOneId))]
        [InverseProperty(nameof(User.FriendFriendOnes))]
        public virtual User FriendOne { get; set; }
        [ForeignKey(nameof(FriendTwoId))]
        [InverseProperty(nameof(User.FriendFriendTwos))]
        public virtual User FriendTwo { get; set; }
    }
}
