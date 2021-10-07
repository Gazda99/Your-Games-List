using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace YGL.Model
{
    [Table("GameMode")]
    public partial class GameMode
    {
        public GameMode()
        {
            GameHasGameModes = new HashSet<GameHasGameMode>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [InverseProperty(nameof(GameHasGameMode.GameMode))]
        public virtual ICollection<GameHasGameMode> GameHasGameModes { get; set; }
    }
}
