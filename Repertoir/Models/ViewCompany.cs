using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Repertoir.Models
{
    public class ViewCompany : ViewContact
    {
        [Required]
        [Display(Name = "Nom société")]
        [StringLength(100)]
        public string CompanyName { get; set; }

        public IList<ContactList> People { get; set; }
    }

    public static class ViewCompanyExtensions
    {
        public static ViewCompany To_ViewCompany(this Contact model)
        {
            var view_model = new ViewCompany
            {
                Contact_ID = model.Contact_ID,
                DisplayName = model.DisplayName,
                CompanyName = model.CompanyName,
                Phone1 = model.Phone1,
                Phone2 = model.Phone2,
                Fax = model.Fax,
                Email = model.Email,
                Url = model.Url,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                PostalCode = model.PostalCode,
                Municipality = model.Municipality,
                Country = model.Country,
                Notes = model.Notes,
                People = new List<ContactList>()
            };

            return view_model;
        }

        public static Contact Update_With_ViewCompany(this Contact model, ViewCompany view_model)
        {
            model.Contact_ID = view_model.Contact_ID;
            model.DisplayName = view_model.CompanyName;
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

        public static IList<ContactList> To_ContactList(this IEnumerable<Contact> model)
        {
            var view_model = (from c in model
                              orderby c.DisplayName
                              select new ContactList
                              {
                                  Contact_ID = c.Contact_ID,
                                  DisplayName = c.DisplayName,
                                  Phone1 = c.Phone1,
                                  Email = c.Email,
                                  Civility = c.Civility,
                                  IsCompany = c.IsCompany,
                                  ControllerName = c.IsCompany ? "Companies" : "People"
                              }).ToList();

            return view_model;
        }
    }
}