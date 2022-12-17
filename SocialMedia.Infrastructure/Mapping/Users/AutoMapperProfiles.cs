namespace SocialMedia.Infrastructure.Mapping.Users;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<CreateUserDTO, User>();
        CreateMap<User, CreateUserDTO>();
        CreateMap<User, UserDTO>();
        CreateMap<UserDTO, User>();
    }
}
