namespace SocialMedia.Core.Services;

public class PostService : IPostService
{
    private readonly IUnitOfWork _unitOfWork;

    public PostService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Post>> GetAsync()
    {
        return await _unitOfWork.postRepository.GetAsync();
    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        Post? post = await _unitOfWork.postRepository.GetByIdAsync(id);
        if (post is null) { return new Post(); }

        User? user = await _unitOfWork.userRepository.GetByIdAsync(post.UserId);
        if (user is null) { return new Post(); }

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
        return post;
    }

    public async Task<Post?> PostAsync(Post create_post)
    {
        User? user = await _unitOfWork.userRepository.GetByIdAsync(create_post.UserId);
        if (user?.Id is 0)
        {
            throw new BusinessException("User does not exist");
        }

        Post? post = await _unitOfWork.postRepository.PostAsync(create_post);
        if (post is null) { return null; }
        post.User = user;

        await _unitOfWork.SaveChangesAsync();

        return post;
    }

    public async Task<Post?> UpdateAsync(Post create_post, int id)
    {
        Post? post = await _unitOfWork.postRepository.UpdateAsync(create_post, id);
        if (post is null) { return null; }

        await _unitOfWork.SaveChangesAsync();

        return post;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        bool deleted = await _unitOfWork.postRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return deleted;
    }
}
