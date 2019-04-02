using AutoMapper;
using Monaco.Web.Models;

namespace Monaco.Web.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Sample, SampleDTO>()
                .ForMember(d => d.SomeValue, opt => opt.MapFrom(s => s.Value + 10));
        }
    }
}
