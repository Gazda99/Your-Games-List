using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace YGL.Model
{
    [Table("RefreshToken")]
    public partial class RefreshToken
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        [Required]
        [Unicode(false)]
        public string Token { get; set; }
        [Required]
        [Unicode(false)]
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ExpiryDate { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("RefreshTokens")]
        public virtual User User { get; set; }
    }
}
