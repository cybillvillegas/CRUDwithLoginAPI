using AutoMapper;
using TNC_API.DTO.Input;
using TNC_API.DTO.Output;
using TNC_API.Models;

namespace TNC_API.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserRequestDTO, User>();
            CreateMap<User, UserResponseDTO>();
        }
    }
}
