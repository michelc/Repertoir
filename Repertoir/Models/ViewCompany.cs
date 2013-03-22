﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
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
            var view_model = Mapper.Map<ViewCompany>(model);

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

        public static ICollection<ContactList> To_ContactList(this IQueryable<Contact> model)
        {
            var view_model = (from c in model
                              orderby c.DisplayName
                              select new ContactList
                              {
                                  Contact_ID = c.Contact_ID,
                                  DisplayName = c.DisplayName,
                                  Phone1 = c.Phone1,
                                  Email = c.Email,
                                  Informations = c.Company_ID.HasValue
                                                    ? c.Title + " // " + c.Company.CompanyName
                                                    : "// " + c.PostalCode + " " + c.Municipality,
                                  Civility = c.Civility,
                                  IsCompany = c.IsCompany,
                                  Slug = c.Slug,
                                  ControllerName = c.IsCompany ? "Companies" : "People"
                              }).ToList();

            return view_model;
        }
    }
}