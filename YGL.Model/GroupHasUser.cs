using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace YGL.Model
{
    [Table("GroupHasUser")]
    public partial class GroupHasUser
    {
        [Key]
        public long Id { get; set; }
        public long GroupId { get; set; }
        public long UserId { get; set; }

        [ForeignKey(nameof(GroupId))]
        [InverseProperty("GroupHasUsers")]
        public virtual Group Group { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("GroupHasUsers")]
        public virtual User User { get; set; }
    }
}
