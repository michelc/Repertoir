using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Repertoir.Models
{
    public class RepertoirContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
    }

    public class Contact
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Nom contact")]
        [StringLength(255)]
        public string ContactName { get; set; }

        [Required]
        [Display(Name = "Téléphone")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(50)]
        public string Phone { get; set; }

        [Display(Name = "Adresse mél")]
        [DataType(DataType.EmailAddress)]
        [StringLength(255)]
        public string Email { get; set; }
    }
}