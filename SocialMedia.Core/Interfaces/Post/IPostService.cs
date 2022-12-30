using SocialMedia.Core.Entities;
namespace SocialMedia.Core.Interfaces.Post;
public interface IPostService
{
    public Task<IEnumerable<Entities.Post>> GetAsync();
    public Task<Entities.Post?> GetByIdAsync(int id);
    public Task<Entities.Post?> PostAsync(Entities.Post post);
    public Task<Entities.Post?> UpdateAsync(Entities.Post post, int id);
    public Task<bool> DeleteAsync(int id);
}