namespace SocialMedia.Core.Interfaces.User;

public interface IUserService
{
    public Task<ValidatedResult<IEnumerable<Entities.User>>> GetAsync();
    public Task<Entities.User?> GetByIdAsync(int id);
    public Task<Entities.User?> PostAsync(Entities.User create_user);
    public Task<Entities.User?> UpdateAsync(Entities.User create_use, int id);
    public Task<bool> DeleteAsync(int id);
}