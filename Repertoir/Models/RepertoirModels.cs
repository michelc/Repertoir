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
        [Display(Name = "Nom")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Display(Name = "Prénom")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Display(Name = "Civilité")]
        [UIHint("Civility")]
        public string Civility { get; set; }

        [Display(Name = "Fonction")]
        [StringLength(100)]
        public string Title { get; set; }

        [Display(Name = "Nom société")]
        [StringLength(100)]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Téléphone")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(50)]
        public string Phone1 { get; set; }

        [Display(Name = "Autre téléphone")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(50)]
        public string Phone2 { get; set; }

        [Display(Name = "Numéro télécopieur")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(50)]
        public string Fax { get; set; }

        [Display(Name = "Adresse mél")]
        [DataType(DataType.EmailAddress)]
        [StringLength(255)]
        public string Email { get; set; }

        [Display(Name = "Site internet")]
        [DataType(DataType.Url)]
        [StringLength(255)]
        public string Url { get; set; }

        [Display(Name = "Adresse")]
        [StringLength(100)]
        public string AddressLine1 { get; set; }

        [Display(Name = "Suite adresse")]
        [StringLength(100)]
        public string AddressLine2 { get; set; }

        [Display(Name = "Code postal")]
        [UIHint("ShortText")]
        [StringLength(20)]
        public string PostalCode { get; set; }

        [Display(Name = "Ville")]
        [StringLength(100)]
        public string Municipality { get; set; }

        [Display(Name = "Pays")]
        [StringLength(100)]
        public string Country { get; set; }

        [Display(Name = "Remarques")]
        [Column(TypeName = "ntext")]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }
    }
}