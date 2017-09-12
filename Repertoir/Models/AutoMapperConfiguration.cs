using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace Repertoir.Models
{
    public static class AutoMap
    {
        private static MapperConfiguration Config { get; set; }
        private static IMapper Mapper { get; set; }
        private static IMapper DynamicMapper { get; set; }

        public static void Configure()
        {
            Config = new MapperConfiguration(cfg => {
                ConfigureEntitiesToViewModels(cfg);
            });

            Mapper = Config.CreateMapper();

            var DynamicConfig = new MapperConfiguration(cfg => cfg.CreateMissingTypeMaps = true);
            DynamicMapper = DynamicConfig.CreateMapper();
        }

        public static T Map<T>(object model)
        {
            var view_model = AutoMap.Mapper.Map<T>(model);

            return view_model;
        }

        public static T DynamicMap<T>(object model)
        {
            var view_model = AutoMap.DynamicMapper.Map<T>(model);

            return view_model;
        }

        private static void ConfigureEntitiesToViewModels(IMapperConfigurationExpression config)
        {
            // Entités vers ViewModels

            config.CreateMap<Tag, ViewTag>();
            config.CreateMap<Tag, ReplaceTag>();

            config.CreateMap<Contact, ViewPerson>()
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

            config.CreateMap<Tag, int>().ConvertUsing(tag => tag.Tag_ID);

            config.CreateMap<Contact, ViewCompany>()
                  .ForMember(
                             company => company.Tags_IDs,
                             opt => opt.MapFrom(contact => contact.Tags)
                            )
                  .ForMember(
                             company => company.People,
                             opt => opt.UseValue(new List<ContactList>())
                            );

            config.CreateMap<Contact, ContactList>()
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

            config.CreateMap<Contact, FlatContact>()
                  .ForMember(
                             flat => flat.Tags,
                             opt => opt.MapFrom(contact => string.Join(",", contact.Tags.Select(t => t.Caption)))
                            )
                  .ForMember(
                             flat => flat.CompanyName,
                             opt => opt.MapFrom(contact => contact.Company_ID.HasValue
                                                           ? contact.CompanyName
                                                           : contact.CompanyName)
                            );
        }
    }
}
