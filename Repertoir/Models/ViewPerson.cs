﻿using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Repertoir.Helpers;

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

        [Display(Name = "Société")]
        public int? Company_ID { get; set; }
        public string CompanySlug { get; set; }

        public SelectList Companies { get; set; }
    }

    public static class ViewPersonExtensions
    {
        public static ViewPerson To_ViewPerson(this Contact model)
        {
            // Conservé temporairement pour les tests unitaires
            // (en attendant de référencer AutoMapper dans le projet Repertoir.Tests)
            var view_model = AutoMap.Map<ViewPerson>(model);

            return view_model;
        }

        public static Contact Update_With_ViewPerson(this Contact model, ViewPerson view_model)
        {
            model.Contact_ID = view_model.Contact_ID;
            model.DisplayName = (view_model.FirstName + " " + view_model.LastName).Trim();
            model.Slug = model.DisplayName.Slugify();
            model.IsCompany = false;
            model.Civility = view_model.Civility;
            model.LastName = view_model.LastName;
            model.FirstName = view_model.FirstName;
            model.Title = view_model.Title;
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
            model.Company_ID = view_model.Company_ID;
            model.CompanyName = null;

            return model;
        }
    }
}