using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace YGL.Model
{
    [Table("CommentGame")]
    public partial class CommentGame
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        public int GameId { get; set; }
        [Required]
        [StringLength(2000)]
        public string Comment { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int Stars { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateAdded { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateUpdated { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [ForeignKey(nameof(GameId))]
        [InverseProperty("CommentGames")]
        public virtual Game Game { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("CommentGames")]
        public virtual User User { get; set; }
    }
}
