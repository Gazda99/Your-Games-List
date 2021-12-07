using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace YGL.Model
{
    [Table("ListEntry")]
    public partial class ListEntry
    {
        [Key]
        public long Id { get; set; }
        public int PlatformId { get; set; }
        public long ListId { get; set; }
        public int GameId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateAdded { get; set; }
        public bool IsWishlisted { get; set; }
        public bool IsCurrentlyPlaying { get; set; }
        public bool IsFinished { get; set; }
        public bool IsOwned { get; set; }
        public bool IsAbandoned { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [ForeignKey(nameof(GameId))]
        [InverseProperty("ListEntries")]
        public virtual Game Game { get; set; }
        [ForeignKey(nameof(ListId))]
        [InverseProperty(nameof(ListOfGame.ListEntries))]
        public virtual ListOfGame List { get; set; }
        [ForeignKey(nameof(PlatformId))]
        [InverseProperty("ListEntries")]
        public virtual Platform Platform { get; set; }
    }
}
