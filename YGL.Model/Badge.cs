using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace YGL.Model
{
    [Table("Badge")]
    public partial class Badge
    {
        public Badge()
        {
            UserHasBadges = new HashSet<UserHasBadge>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string Name { get; set; }
        [Required]
        [StringLength(1500)]
        [Unicode(false)]
        public string Description { get; set; }
        public byte Rarity { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [InverseProperty(nameof(UserHasBadge.Badge))]
        public virtual ICollection<UserHasBadge> UserHasBadges { get; set; }
    }
}
