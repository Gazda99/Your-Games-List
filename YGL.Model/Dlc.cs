using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace YGL.Model
{
    [Table("Dlc")]
    public partial class Dlc
    {
        [Key]
        public long Id { get; set; }
        public int GameBaseId { get; set; }
        public int GameDlcId { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [ForeignKey(nameof(GameBaseId))]
        [InverseProperty(nameof(Game.DlcGameBases))]
        public virtual Game GameBase { get; set; }
        [ForeignKey(nameof(GameDlcId))]
        [InverseProperty(nameof(Game.DlcGameDlcs))]
        public virtual Game GameDlc { get; set; }
    }
}
