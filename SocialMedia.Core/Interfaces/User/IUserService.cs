namespace SocialMedia.Core.Interfaces.User;

public interface IUserService
{
    public Task<ValidatedResult<IEnumerable<Entities.User>>> GetAsync();
    public Task<ValidatedResult<Entities.User>> GetByIdAsync(int id);
    public Task<ValidatedResult<Entities.User>> PostAsync(Entities.User create_user);
    public Task<ValidatedResult<Entities.User>> UpdateAsync(Entities.User create_use, int id);
    public Task<ValidatedResult<bool>> DeleteAsync(int id);
}