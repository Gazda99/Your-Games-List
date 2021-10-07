using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace YGL.Model
{
    [Table("GameWithAgeCategory")]
    public partial class GameWithAgeCategory
    {
        [Key]
        public long Id { get; set; }
        public byte Category { get; set; }
        public byte Rating { get; set; }
        public int GameId { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [ForeignKey(nameof(GameId))]
        [InverseProperty("GameWithAgeCategories")]
        public virtual Game Game { get; set; }
    }
}
