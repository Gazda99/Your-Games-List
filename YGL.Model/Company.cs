using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

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
        public string Description { get; set; }
        [Required]
        public string Name { get; set; }
        public short Country { get; set; }
        [Required]
        public bool? ItemStatus { get; set; }

        [InverseProperty(nameof(GameHasCompany.Company))]
        public virtual ICollection<GameHasCompany> GameHasCompanies { get; set; }
    }
}
