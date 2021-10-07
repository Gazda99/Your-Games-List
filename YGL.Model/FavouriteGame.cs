using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace YGL.Model
{
    [Table("FavouriteGame")]
    public partial class FavouriteGame
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        public int GameId { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [ForeignKey(nameof(GameId))]
        [InverseProperty("FavouriteGames")]
        public virtual Game Game { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("FavouriteGames")]
        public virtual User User { get; set; }
    }
}
