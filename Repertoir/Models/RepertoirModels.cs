using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace Repertoir.Models
{
    public class Contact
    {
        [Key]
        public int Contact_ID { get; set; }

        // 1 personne peut faire parti d'une société
        public int? Company_ID { get; set; }
        [ForeignKey("Company_ID")]
        public virtual Contact Company { get; set; }

        // 1 société regroupe plusieurs personnes
        public virtual ICollection<Contact> People { get; set; }

        [StringLength(100)]
        public string CompanyName { get; set; }

        public bool IsCompany { get; set; } // true = Société / false = Personne

        [Required]
        [StringLength(100)]
        public string DisplayName { get; set; }

        [StringLength(100)]
        public string Slug { get; set; }

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

        // 1 contact peut avoir plusieurs tags
        public virtual ICollection<Tag> Tags { get; set; }
    }

    public class Tag
    {
        [Key]
        public int Tag_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Caption { get; set; }

        // 1 tag peut s'appliquer à plusieurs contacts
        public virtual ICollection<Contact> Contacts { get; set; }
    }

    public class RepertoirContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<RepertoirContext, Configuration>());

            // Le lien Many-To-Many entre les tables Contacts et Tags (et la table
            // qui sert à enregistrer cette association) est généré automatiquement
            // grace à la présence des 2 propriétés suivantes :
            // * Contact class : public virtual ICollection<Tag> Tags { get; set; }
            // * Tag class     : public virtual ICollection<Contact> Contacts { get; set; }
            //
            // Utilisation de Fluid API pour spécifier
            // * le nom de la table :
            //   - Contacts_Tags et pas ContactTags
            // * le nom des colonnes :
            //   - Contact_ID et pas Contact_Contact_ID
            //   - Tag_ID et pas Tag_Tag_ID
            modelBuilder.Entity<Contact>()
                .HasMany(contact => contact.Tags)
                .WithMany(tag => tag.Contacts)
                .Map(x =>
                {
                    x.ToTable("Contacts_Tags");
                    x.MapLeftKey("Contact_ID");
                    x.MapRightKey("Tag_ID");
                });
        }
    }

    public class Configuration : DbMigrationsConfiguration<RepertoirContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}