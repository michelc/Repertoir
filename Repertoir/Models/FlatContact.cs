using Repertoir.Helpers;

namespace Repertoir.Models
{
    public class FlatContact
    {
        public string Slug { get; set; }
        public string DisplayName { get; set; }
        public bool IsCompany { get; set; }
        public string CompanyName { get; set; }
        public string Civility { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Title { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostalCode { get; set; }
        public string Municipality { get; set; }
        public string Country { get; set; }
        public string Notes { get; set; }
    }

    public static class FlatContactExtensions
    {
        public static FlatContact To_FlatContact(this Contact model)
        {
            var view_model = new FlatContact
            {
                Slug = model.Slug,
                DisplayName = model.DisplayName,
                IsCompany = model.IsCompany,
                CompanyName = model.Company_ID.HasValue ? model.Company.CompanyName : model.CompanyName,
                Civility = model.Civility,
                LastName = model.LastName,
                FirstName = model.FirstName,
                Title = model.Title,
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

        public static Contact Update_With_FlatContact(this Contact model, FlatContact view_model)
        {
            model.DisplayName = view_model.DisplayName;
            model.Slug = model.DisplayName.Slugify();
            model.IsCompany = view_model.IsCompany;
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