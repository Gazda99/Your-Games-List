using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace YGL.Model
{
    [Table("GameHasPlayerPerspective")]
    public partial class GameHasPlayerPerspective
    {
        [Key]
        public long Id { get; set; }
        public int GameId { get; set; }
        public int PlayerPerspectiveId { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [ForeignKey(nameof(GameId))]
        [InverseProperty("GameHasPlayerPerspectives")]
        public virtual Game Game { get; set; }
        [ForeignKey(nameof(PlayerPerspectiveId))]
        [InverseProperty("GameHasPlayerPerspectives")]
        public virtual PlayerPerspective PlayerPerspective { get; set; }
    }
}
