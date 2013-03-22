using AutoMapper;
using System.Collections.Generic;

namespace Repertoir.Models
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            // Entités vers ViewModels

            Mapper.CreateMap<Tag, ViewTag>();

            Mapper.CreateMap<Contact, ViewPerson>()
                  .ForMember(
                             person => person.Tags_IDs,
                             opt => opt.MapFrom(contact => contact.Tags)
                            )
                  .ForMember(
                             person => person.CompanyName, 
                             opt => opt.MapFrom(contact => contact.Company_ID.HasValue 
                                                           ? contact.Company.CompanyName
                                                           : null)
                            );

            Mapper.CreateMap<Tag, int>().ConvertUsing(tag => tag.Tag_ID);

            Mapper.CreateMap<Contact, ViewCompany>()
                  .ForMember(
                             company => company.Tags_IDs,
                             opt => opt.MapFrom(contact => contact.Tags)
                            )
                  .ForMember(
                             company => company.People,
                             opt => opt.UseValue(new List<ContactList>())
                            );

            Mapper.CreateMap<Contact, ContactList>()
                  .ForMember(
                             line => line.ControllerName,
                             opt => opt.MapFrom(contact => contact.IsCompany ? "Companies" : "People")
                            )
                  .ForMember(
                             line => line.Informations,
                             opt => opt.MapFrom(contact => contact.Company_ID.HasValue
                                                           ? contact.Title + " // " + contact.CompanyName
                                                           : "// " + contact.PostalCode + " " + contact.Municipality)
                            );

        }
    }
}