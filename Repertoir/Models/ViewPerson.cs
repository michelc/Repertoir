using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Repertoir.Models
{
    public class ViewPerson : ViewContact
    {
        [Required]
        [Display(Name = "Nom")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Display(Name = "Prénom")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Display(Name = "Civilité")]
        [UIHint("Civility")]
        [StringLength(3)]
        public string Civility { get; set; }

        [Display(Name = "Fonction")]
        [StringLength(100)]
        public string Title { get; set; }

        [Display(Name = "Nom société")]
        [StringLength(100)]
        public string CompanyName { get; set; }
    }

    public static class ViewPersonExtensions
    {
        public static ViewPerson To_ViewPerson(this Contact model)
        {
            var view_model = new ViewPerson
            {
                Contact_ID = model.Contact_ID,
                DisplayName = model.DisplayName,
                Civility = model.Civility,
                LastName = model.LastName,
                FirstName = model.FirstName,
                Title = model.Title,
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
                Notes = model.Notes
            };

            return view_model;
        }

        public static Contact Update_With_ViewPerson(this Contact model, ViewPerson view_model)
        {
            model.Contact_ID = view_model.Contact_ID;
            model.DisplayName = (view_model.FirstName + " " + view_model.LastName).Trim();
            model.Civility = view_model.Civility;
            model.LastName = view_model.LastName;
            model.FirstName = view_model.FirstName;
            model.Title = view_model.Title;
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
    }
}