using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace YGL.Model
{
    [Table("PlayerPerspective")]
    public partial class PlayerPerspective
    {
        public PlayerPerspective()
        {
            GameHasPlayerPerspectives = new HashSet<GameHasPlayerPerspective>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [InverseProperty(nameof(GameHasPlayerPerspective.PlayerPerspective))]
        public virtual ICollection<GameHasPlayerPerspective> GameHasPlayerPerspectives { get; set; }
    }
}
