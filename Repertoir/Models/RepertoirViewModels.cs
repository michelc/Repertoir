using System.ComponentModel.DataAnnotations;

namespace Repertoir.Models
{
    /* Contact simplifié utilisé pour /Contacts/Index */
    public class ContactList
    {
        public int Contact_ID { get; set; }

        [Display(Name = "Nom")]
        public string DisplayName { get; set; }

        [Display(Name = "Téléphone")]
        public string Phone1 { get; set; }

        [Display(Name = "Adresse mél")]
        public string Email { get; set; }

        public string Civility { get; set; }
        public bool IsCompany { get; set; }
        public string ControllerName { get; set; }
    }
}