using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace YGL.Model
{
    [Table("GameHasCompany")]
    public partial class GameHasCompany
    {
        [Key]
        public long Id { get; set; }
        public int GameId { get; set; }
        public int CompanyId { get; set; }
        public bool IsDeveloper { get; set; }
        public bool IsPorting { get; set; }
        public bool IsPublisheir { get; set; }
        public bool IsSupporting { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [InverseProperty("GameHasCompanies")]
        public virtual Company Company { get; set; }
        [ForeignKey(nameof(GameId))]
        [InverseProperty("GameHasCompanies")]
        public virtual Game Game { get; set; }
    }
}
