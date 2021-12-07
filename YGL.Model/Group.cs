using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace YGL.Model
{
    [Table("Group")]
    public partial class Group
    {
        public Group()
        {
            GroupHasUsers = new HashSet<GroupHasUser>();
        }

        [Key]
        public long Id { get; set; }
        public long CreatorId { get; set; }
        [Required]
        [StringLength(128)]
        [Unicode(false)]
        public string Name { get; set; }
        [Required]
        [StringLength(2000)]
        [Unicode(false)]
        public string Description { get; set; }
        [StringLength(128)]
        [Unicode(false)]
        public string Slug { get; set; }
        public short UsersAmount { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [ForeignKey(nameof(CreatorId))]
        [InverseProperty(nameof(User.Groups))]
        public virtual User Creator { get; set; }
        [InverseProperty(nameof(GroupHasUser.Group))]
        public virtual ICollection<GroupHasUser> GroupHasUsers { get; set; }
    }
}
