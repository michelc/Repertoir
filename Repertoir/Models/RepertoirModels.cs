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

        public string DisplayName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(3)]
        public string Civility { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(100)]
        public string CompanyName { get; set; }

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