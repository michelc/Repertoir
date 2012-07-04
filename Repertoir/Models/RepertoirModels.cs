using System.Collections.Generic;
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
        [Key]
        public int Contact_ID { get; set; }

        // 1 personne peut faire parti d'une société
        public int? Company_ID { get; set; }
        [ForeignKey("Company_ID")]
        public virtual Contact Company { get; set; }
        
        // 1 société regroupe plusieurs personnes
        public virtual IList<Contact> People { get; set; }

        [StringLength(100)]
        public string CompanyName { get; set; }

        [StringLength(100)]
        public string DisplayName { get; set; }

        public bool IsCompany { get; set; } // true = Société / false = Personne

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(3)]
        public string Civility { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        public string Phone1 { get; set; }

        [StringLength(50)]
        public string Phone2 { get; set; }

        [StringLength(50)]
        public string Fax { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Url { get; set; }

        [StringLength(100)]
        public string AddressLine1 { get; set; }

        [StringLength(100)]
        public string AddressLine2 { get; set; }

        [StringLength(20)]
        public string PostalCode { get; set; }

        [StringLength(100)]
        public string Municipality { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

        [Column(TypeName = "ntext")]
        public string Notes { get; set; }
    }
}