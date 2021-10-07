using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace YGL.Model
{
    [Table("UserWithRole")]
    public partial class UserWithRole
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        public short Role { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserWithRoles")]
        public virtual User User { get; set; }
    }
}
