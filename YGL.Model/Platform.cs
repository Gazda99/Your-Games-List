using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace YGL.Model
{
    [Table("Platform")]
    public partial class Platform
    {
        public Platform()
        {
            GameAndPlatformWithReleaseDates = new HashSet<GameAndPlatformWithReleaseDate>();
            ListEntries = new HashSet<ListEntry>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [Unicode(false)]
        public string Abbr { get; set; }
        [Required]
        [Unicode(false)]
        public string Name { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [InverseProperty(nameof(GameAndPlatformWithReleaseDate.Platform))]
        public virtual ICollection<GameAndPlatformWithReleaseDate> GameAndPlatformWithReleaseDates { get; set; }
        [InverseProperty(nameof(ListEntry.Platform))]
        public virtual ICollection<ListEntry> ListEntries { get; set; }
    }
}
