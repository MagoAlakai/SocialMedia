using SocialMedia.Core.DTOs.Posts;
using SocialMedia.Core.Entities;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories.Users;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    public UserRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }
    public async Task<UserDTO?> PostAsync(CreateUserDTO create_user_dto)
    {
        bool user_exist = await _applicationDbContext.Users.AnyAsync(x => x.Email == create_user_dto.Email);
        if (user_exist is true) { return null; }

        User? user = _mapper.Map<User>(create_user_dto);
        if (user == null) { return null;}

        _applicationDbContext.Add(user);
        await _applicationDbContext.SaveChangesAsync();

        UserDTO? user_dto = _mapper.Map<UserDTO>(user);

        return user_dto;
    }
}
