using AutoMapper;

namespace DDDPlayGround.Application.Authentication.Dtos
{
    public class AuthenticationProfile : Profile
    {
        public AuthenticationProfile()
        {
            CreateMap<RegisterRequestDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.Tokens, opt => opt.Ignore());

            CreateMap<User, LoginResponseDto>()
                .ForMember(dest => dest.Token, opt => opt.Ignore());
        }
    }
}
