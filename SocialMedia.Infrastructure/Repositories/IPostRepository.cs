namespace SocialMedia.Infrastructure.Repositories;

public interface IPostRepository
{
    public Task<IEnumerable<Post>> GetPosts();
}
