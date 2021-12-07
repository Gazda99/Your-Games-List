using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace YGL.Model
{
    [Table("Theme")]
    public partial class Theme
    {
        public Theme()
        {
            GameHasThemes = new HashSet<GameHasTheme>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [Unicode(false)]
        public string Name { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [InverseProperty(nameof(GameHasTheme.Theme))]
        public virtual ICollection<GameHasTheme> GameHasThemes { get; set; }
    }
}
