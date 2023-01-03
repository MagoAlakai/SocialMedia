namespace SocialMedia.Core.Interfaces.Post;
public interface IPostService
{
    public Task<ValidatedResult<IEnumerable<Entities.Post>>> GetAsync();
    public Task<ValidatedResult<Entities.Post>> GetByIdAsync(int id);
    public Task<ValidatedResult<Entities.Post>> PostAsync(Entities.Post post);
    public Task<ValidatedResult<Entities.Post>> UpdateAsync(Entities.Post post, int id);
    public Task<ValidatedResult<bool>> DeleteAsync(int id);
}