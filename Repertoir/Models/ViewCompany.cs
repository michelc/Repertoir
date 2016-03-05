using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Repertoir.Helpers;

namespace Repertoir.Models
{
    public class ViewCompany : ViewContact
    {
        [Required]
        [Display(Name = "Nom société")]
        [StringLength(100)]
        public string CompanyName { get; set; }

        public ICollection<ContactList> People { get; set; }
    }

    public static class ViewCompanyExtensions
    {
        public static ViewCompany To_ViewCompany(this Contact model)
        {
            // Conservé temporairement pour les tests unitaires
            // (en attendant de référencer AutoMapper dans le projet Repertoir.Tests)
            var view_model = AutoMap.Map<ViewCompany>(model);

            return view_model;
        }

        public static Contact Update_With_ViewCompany(this Contact model, ViewCompany view_model)
        {
            model.Contact_ID = view_model.Contact_ID;
            model.DisplayName = view_model.CompanyName.Trim();
            model.Slug = model.DisplayName.Slugify();
            model.IsCompany = true;
            model.Civility = null;
            model.LastName = null;
            model.FirstName = null;
            model.Title = null;
            model.CompanyName = view_model.CompanyName;
            model.Phone1 = view_model.Phone1;
            model.Phone2 = view_model.Phone2;
            model.Fax = view_model.Fax;
            model.Email = view_model.Email;
            model.Url = view_model.Url;
            model.AddressLine1 = view_model.AddressLine1;
            model.AddressLine2 = view_model.AddressLine2;
            model.PostalCode = view_model.PostalCode;
            model.Municipality = view_model.Municipality;
            model.Country = view_model.Country;
            model.Notes = view_model.Notes;

            return model;
        }

        public static IList<ContactList> List(this IQueryable<Contact> model, int Parent_ID = 0)
        {
            // Tous les contacts ou uniquement ceux de la société en cours
            var query = Parent_ID == 0
                        ? model
                        : model.Where(contact => contact.Company_ID == Parent_ID);

            // Tri et sélection des colonnes nécessaires uniquement
            // (dans une liste d'objets anonymes car "select new Contact" n'est pas possible)
            var list = (from c in query
                        orderby c.DisplayName
                        select new
                        {
                            Contact_ID = c.Contact_ID,
                            DisplayName = c.DisplayName,
                            Phone1 = c.Phone1,
                            Email = c.Email,
                            Company_ID = c.Company_ID,
                            CompanyName = c.Company_ID.HasValue ? c.Company.CompanyName : c.CompanyName,
                            Title = c.Title,
                            PostalCode = c.PostalCode,
                            Municipality = c.Municipality,
                            Civility = c.Civility,
                            IsCompany = c.IsCompany,
                            Slug = c.Slug
                        }).ToList();

            // Transforme la liste anonyme en liste de contacts
            var contacts = list.Select(a => AutoMap.DynamicMap<Contact>(a)).ToList();
            // abrégeable en list.Select(Mapper.DynamicMap<Contact>).ToList();

            // Renvoie une liste d'objets ViewModel
            return AutoMap.Map<IList<ContactList>>(contacts);
        }
    }
}