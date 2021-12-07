using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace YGL.Model
{
    [Table("GameAndPlatformWithReleaseDate")]
    public partial class GameAndPlatformWithReleaseDate
    {
        [Key]
        public long Id { get; set; }
        public int PlatformId { get; set; }
        public int GameId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ReleaseDate { get; set; }
        public short Region { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [ForeignKey(nameof(GameId))]
        [InverseProperty("GameAndPlatformWithReleaseDates")]
        public virtual Game Game { get; set; }
        [ForeignKey(nameof(PlatformId))]
        [InverseProperty("GameAndPlatformWithReleaseDates")]
        public virtual Platform Platform { get; set; }
    }
}
