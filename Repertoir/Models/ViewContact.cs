using System.ComponentModel.DataAnnotations;

namespace Repertoir.Models
{
    public class ViewContact
    {
        public int ID { get; set; }

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
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Cette adresse mél n'est pas correcte")]
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