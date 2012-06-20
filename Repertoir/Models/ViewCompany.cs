using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Repertoir.Models
{
    public class ViewCompany
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Nom société")]
        [StringLength(100)]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Téléphone")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(50)]
        public string Phone1 { get; set; }

        [Display(Name = "Autre téléphone")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(50)]
        public string Phone2 { get; set; }

        [Display(Name = "Numéro télécopieur")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(50)]
        public string Fax { get; set; }

        [Display(Name = "Adresse mél")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Cette adresse mél n'est pas correcte")]
        [DataType(DataType.EmailAddress)]
        [StringLength(255)]
        public string Email { get; set; }

        [Display(Name = "Site internet")]
        [DataType(DataType.Url)]
        [StringLength(255)]
        public string Url { get; set; }

        [Display(Name = "Adresse")]
        [StringLength(100)]
        public string AddressLine1 { get; set; }

        [Display(Name = "Suite adresse")]
        [StringLength(100)]
        public string AddressLine2 { get; set; }

        [Display(Name = "Code postal")]
        [StringLength(20)]
        public string PostalCode { get; set; }

        [Display(Name = "Ville")]
        [StringLength(100)]
        public string Municipality { get; set; }

        [Display(Name = "Pays")]
        [StringLength(100)]
        public string Country { get; set; }

        [Display(Name = "Remarques")]
        [Column(TypeName = "ntext")]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }
    }

    public static class ViewCompanyExtensions
    {
        public static ViewCompany To_ViewCompany(this Contact model)
        {
            var view_model = new ViewCompany
            {
                ID = model.ID,
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

        public static Contact Update_With_ViewCompany(this Contact model, ViewCompany view_model)
        {
            model.ID = view_model.ID;
            model.Civility = null;
            model.LastName = "*";
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
    }
}