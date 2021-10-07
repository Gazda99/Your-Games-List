using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace YGL.Model
{
    [Table("EmailConfirmation")]
    public partial class EmailConfirmation
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Url { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ExpiryDate { get; set; }
        public long UserId { get; set; }
        public bool IsUsed { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("EmailConfirmations")]
        public virtual User User { get; set; }
    }
}
