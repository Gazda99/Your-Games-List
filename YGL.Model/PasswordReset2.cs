using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace YGL.Model
{
    [Table("PasswordReset2")]
    public partial class PasswordReset2
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [Unicode(false)]
        public string Token { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ExpiryDate { get; set; }
        public long UserId { get; set; }
        public bool IsUsed { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("PasswordReset2s")]
        public virtual User User { get; set; }
    }
}
