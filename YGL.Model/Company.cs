using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace YGL.Model
{
    [Table("Company")]
    public partial class Company
    {
        public Company()
        {
            GameHasCompanies = new HashSet<GameHasCompany>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [Unicode(false)]
        public string Description { get; set; }
        [Required]
        [Unicode(false)]
        public string Name { get; set; }
        public short Country { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [InverseProperty(nameof(GameHasCompany.Company))]
        public virtual ICollection<GameHasCompany> GameHasCompanies { get; set; }
    }
}
