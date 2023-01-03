namespace SocialMedia.Core.Services;

public class PostService : IPostService
{
    private readonly IUnitOfWork _unitOfWork;

    public PostService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ValidatedResult<IEnumerable<Post>>> GetAsync()
    {
        IEnumerable<Post>? result = await _unitOfWork.postRepository.GetAsync();
        if (result.Any() is false) { return ValidatedResult<IEnumerable<Post>>.Failed(0, "There are no Posts registered"); }

        return ValidatedResult<IEnumerable<Post>>.Passed(result);
    }

    public async Task<ValidatedResult<Post>> GetByIdAsync(int id)
    {
        Post? post = await _unitOfWork.postRepository.GetByIdAsync(id);
        if (post is null) { return ValidatedResult<Post>.Failed(0, "This Post is not registered"); }

        User? user = await _unitOfWork.userRepository.GetByIdAsync(post.UserId);
        if (user is null) { return ValidatedResult<Post>.Failed(0, "This Post has no user "); }

        List<Comment> comments = new();
        comments = _unitOfWork.commentRepository.GetAsync().Result
            .Where(x => x.UserId == post.UserId && x.Active == true)
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

        post.User = user;
        post.Comments = comments;

        return ValidatedResult<Post>.Passed(post);
    }

    public async Task<ValidatedResult<Post>> PostAsync(Post create_post)
    {
        User? user = await _unitOfWork.userRepository.GetByIdAsync(create_post.UserId);
        if (user is null)
        {
            throw new BusinessException("User does not exist");
        }

        Post? post = await _unitOfWork.postRepository.PostAsync(create_post);
        if (post is null) { return ValidatedResult<Post>.Failed(0, "This Post has not been registered"); }
        post.User = user;

        await _unitOfWork.SaveChangesAsync();

        return ValidatedResult<Post>.Passed(post);
    }

    public async Task<ValidatedResult<Post>> UpdateAsync(Post create_post, int id)
    {
        Post? post = await _unitOfWork.postRepository.UpdateAsync(create_post, id);
        if (post is null) { return ValidatedResult<Post>.Failed(0, "This Post is not registered"); }

        await _unitOfWork.SaveChangesAsync();

        return ValidatedResult<Post>.Passed(post); ;
    }

    public async Task<ValidatedResult<bool>> DeleteAsync(int id)
    {
        bool deleted = await _unitOfWork.postRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return ValidatedResult<bool>.Passed(deleted);
    }
}
