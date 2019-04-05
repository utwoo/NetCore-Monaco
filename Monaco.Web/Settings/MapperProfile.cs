using AutoMapper;
using Monaco.Web.Models;

namespace Monaco.Web.Mappers.Settings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Sample, SampleDTO>()
                .ForMember(d => d.SomeValue, opt => opt.MapFrom(s => s.Value + 10));
        }
    }
}
