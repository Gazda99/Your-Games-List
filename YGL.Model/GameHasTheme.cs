using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace YGL.Model
{
    [Table("GameHasTheme")]
    public partial class GameHasTheme
    {
        [Key]
        public long Id { get; set; }
        public int GameId { get; set; }
        public int ThemeId { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [ForeignKey(nameof(GameId))]
        [InverseProperty("GameHasThemes")]
        public virtual Game Game { get; set; }
        [ForeignKey(nameof(ThemeId))]
        [InverseProperty("GameHasThemes")]
        public virtual Theme Theme { get; set; }
    }
}
