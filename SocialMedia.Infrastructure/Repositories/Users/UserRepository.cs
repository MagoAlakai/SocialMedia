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

    public async Task<IEnumerable<UserDTO>> GetAsync()
    {
        IEnumerable<User> users = await _applicationDbContext.Users.ToListAsync();
        if (users is null) { return new List<UserDTO>(); }

        List<UserDTO> user_dto_list = new(_mapper.Map<IEnumerable<UserDTO>>(users));

        return await Task.FromResult(user_dto_list);
    }

    public async Task<UserWithPostsAndCommentsDTO> GetByIdAsync(int id)
    {
        User? user = await _applicationDbContext.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (user is null) { return new UserWithPostsAndCommentsDTO(); }

        List<PostSimplifiedDTO> post_list = new();
        post_list = _applicationDbContext.Posts
            .Where(x => x.UserId == id)
            .Select(x => new PostSimplifiedDTO()
            {
                Id = x.Id,
                Description = x.Description
            })
            .ToList();

        List<CommentSimplifiedDTO> comment_list = new();
        comment_list = _applicationDbContext.Comments
            .Where(x => x.UserId == id)
            .Select(x => new CommentSimplifiedDTO()
            {
                Id = x.Id,
                Description = x.Description
            })
            .ToList();

        UserWithPostsAndCommentsDTO user_with_posts_and_comments_dto = _mapper.Map<User, UserWithPostsAndCommentsDTO>(user, options =>
               options.AfterMap((src, dest) => {
                   dest.Posts = post_list;
                   dest.Comments = comment_list;
               }));

        return await Task.FromResult(user_with_posts_and_comments_dto);
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

    public async Task<UserDTO?> UpdateAsync(CreateUserDTO update_user_dto, int id)
    {
        bool user_exist = await _applicationDbContext.Users.AnyAsync(x => x.Id == id);
        if (user_exist is false) { return null; }

        User? user = _mapper.Map<CreateUserDTO, User>(update_user_dto, options =>
        options.AfterMap((src, dest) =>
        {
            dest.Id = id;
        }));

        _applicationDbContext.Update(user);
        await _applicationDbContext.SaveChangesAsync();

        UserDTO? user_dto = _mapper.Map<UserDTO>(user);

        return user_dto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
    User? user = await _applicationDbContext.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
    if (user is null) { return false; }

    _applicationDbContext.Remove(user);
    await _applicationDbContext.SaveChangesAsync();

    return true;
    }
}
