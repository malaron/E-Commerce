using AutoMapper;
using SharedContracts;

namespace SharedContracts.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<string, Guid>().ConvertUsing(s => Guid.Parse(s));
            CreateMap<string, Guid?>().ConvertUsing(s => String.IsNullOrWhiteSpace(s) ? (Guid?)null : Guid.Parse(s));
            CreateMap<Guid?, string>().ConvertUsing(g => g == null ? null! : g.Value.ToString("N"));
            CreateMap<Guid, string>().ConvertUsing(g => g.ToString("N"));

            CreateMap<ApplicationUser, UserDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)));

            CreateMap<UserDTO, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
        }
    }
}
