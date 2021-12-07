using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace YGL.Model
{
    [Table("UserHasBadge")]
    public partial class UserHasBadge
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        public int BadgeId { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [ForeignKey(nameof(BadgeId))]
        [InverseProperty("UserHasBadges")]
        public virtual Badge Badge { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserHasBadges")]
        public virtual User User { get; set; }
    }
}
