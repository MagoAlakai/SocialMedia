namespace SocialMedia.Core.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ValidatedResult<IEnumerable<User>>> GetAsync()
    {
        IEnumerable<User>? result = await _unitOfWork.userRepository.GetAsync();
        if (result is null) { return ValidatedResult<IEnumerable<User>>.Failed(0, "There are no Users registered"); }

        return ValidatedResult<IEnumerable<User>>.Passed(result);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        User? user = await _unitOfWork.userRepository.GetByIdAsync(id);
        if (user is null) { return new User(); }

        List<Post> posts = new();
        posts = _unitOfWork.postRepository.GetAsync().Result
            .Where(x => x.UserId == user.Id)
            .Select(x => new Post()
            {
                Id = x.Id,
                Comments = x.Comments,
                Date = x.Date,
                Description = x.Description,
                Image = x.Image,
                User = user,
                UserId = user.Id
            }).ToList();

        List<Comment> comments = new();
        comments = _unitOfWork.commentRepository.GetAsync().Result
            .Where(x => x.UserId == user.Id && x.Active == true)
            .Select(x => new Comment()
            {
                Id = x.Id,
                Description = x.Description,
                Active = x.Active,
                Date = x.Date,
                PostId = x.PostId,
                UserId = user.Id
            })
            .ToList();

        user.Posts = posts;
        user.Comments = comments;

        return await _unitOfWork.userRepository.GetByIdAsync(id);
    }

    public async Task<User?> PostAsync(User create_user)
    {
        User? user = await _unitOfWork.userRepository.PostAsync(create_user);
        if (user is null) { return null; }

        await _unitOfWork.SaveChangesAsync();

        return user;
    }

    public async Task<User?> UpdateAsync(User create_user, int id)
    {
        User? user = await _unitOfWork.userRepository.UpdateAsync(create_user, id);
        if (user is null) { return null; }

        await _unitOfWork.SaveChangesAsync();

        return user;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        bool deleted = await _unitOfWork.userRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return deleted;
    }
}
