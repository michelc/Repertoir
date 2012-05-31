using System.ComponentModel.DataAnnotations;

namespace Repertoir.Models
{
    /* Contact simplifié utilisé pour /Contacts/Index */
    public class ContactList
    {
        public int ID { get; set; }

        [Display(Name = "Nom")]
        public string LastName { get; set; }

        [Display(Name = "Prénom")]
        public string FirstName { get; set; }

        [Display(Name = "Téléphone")]
        public string Phone1 { get; set; }

        [Display(Name = "Adresse mél")]
        public string Email { get; set; }
    }
}