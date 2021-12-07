using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace YGL.Model
{
    [Table("GameHasGenre")]
    public partial class GameHasGenre
    {
        [Key]
        public long Id { get; set; }
        public int GameId { get; set; }
        public int GenreId { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [ForeignKey(nameof(GameId))]
        [InverseProperty("GameHasGenres")]
        public virtual Game Game { get; set; }
        [ForeignKey(nameof(GenreId))]
        [InverseProperty("GameHasGenres")]
        public virtual Genre Genre { get; set; }
    }
}
