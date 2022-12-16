using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories;

public class PostRepository : IPostRepository 
{
    private readonly ApplicationDbContext _applicationDbContext;

    public PostRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<IEnumerable<Post>> GetPosts()
    {
        IEnumerable<Post> posts = await _applicationDbContext.Posts.ToListAsync();
        if (posts is null) { return new List<Post>(); }

        return await Task.FromResult(posts);
    }
}
