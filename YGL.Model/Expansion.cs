using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace YGL.Model
{
    [Table("Expansion")]
    public partial class Expansion
    {
        [Key]
        public long Id { get; set; }
        public int GameBaseId { get; set; }
        public int GameExpansionId { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [ForeignKey(nameof(GameBaseId))]
        [InverseProperty(nameof(Game.ExpansionGameBases))]
        public virtual Game GameBase { get; set; }
        [ForeignKey(nameof(GameExpansionId))]
        [InverseProperty(nameof(Game.ExpansionGameExpansions))]
        public virtual Game GameExpansion { get; set; }
    }
}
