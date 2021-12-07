using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace YGL.Model
{
    [Table("GameHasGameMode")]
    public partial class GameHasGameMode
    {
        [Key]
        public long Id { get; set; }
        public int GameId { get; set; }
        public int GameModeId { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [ForeignKey(nameof(GameId))]
        [InverseProperty("GameHasGameModes")]
        public virtual Game Game { get; set; }
        [ForeignKey(nameof(GameModeId))]
        [InverseProperty("GameHasGameModes")]
        public virtual GameMode GameMode { get; set; }
    }
}
