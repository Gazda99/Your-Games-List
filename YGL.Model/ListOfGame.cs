using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace YGL.Model
{
    public partial class ListOfGame
    {
        public ListOfGame()
        {
            ListEntries = new HashSet<ListEntry>();
        }

        [Key]
        public long Id { get; set; }
        public long OwnerId { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }
        [Required]
        [StringLength(1000)]
        [Unicode(false)]
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        public bool IsPublic { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(User.ListOfGames))]
        public virtual User Owner { get; set; }
        [InverseProperty(nameof(ListEntry.List))]
        public virtual ICollection<ListEntry> ListEntries { get; set; }
    }
}
