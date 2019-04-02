using AutoMapper;
using Monaco.Web.Models;

namespace Monaco.Web.Mappers.Configurations
{
    public class MapperConfiguration : Profile
    {
        public MapperConfiguration()
        {
            CreateMap<Sample, SampleDTO>()
                .ForMember(d => d.SomeValue, opt => opt.MapFrom(s => s.Value + 10));
        }
    }
}
