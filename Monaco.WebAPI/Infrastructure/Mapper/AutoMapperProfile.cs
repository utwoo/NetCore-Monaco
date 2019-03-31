using AutoMapper;
using Monaco.WebAPI.Models;

namespace Monaco.WebAPI.Infrastructure.Mapper
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
