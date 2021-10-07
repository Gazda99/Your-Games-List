using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace YGL.Model
{
    [Table("Genre")]
    public partial class Genre
    {
        public Genre()
        {
            GameHasGenres = new HashSet<GameHasGenre>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [InverseProperty(nameof(GameHasGenre.Genre))]
        public virtual ICollection<GameHasGenre> GameHasGenres { get; set; }
    }
}
