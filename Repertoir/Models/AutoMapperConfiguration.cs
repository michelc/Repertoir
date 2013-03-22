using AutoMapper;

namespace Repertoir.Models
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            // Entités vers ViewModels

            Mapper.CreateMap<Tag, ViewTag>();
        }
    }
}