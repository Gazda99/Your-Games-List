using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace YGL.Model
{
    [Table("GameWithWebsite")]
    public partial class GameWithWebsite
    {
        [Key]
        public long Id { get; set; }
        public int GameId { get; set; }
        public byte Category { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [ForeignKey(nameof(GameId))]
        [InverseProperty("GameWithWebsites")]
        public virtual Game Game { get; set; }
    }
}
