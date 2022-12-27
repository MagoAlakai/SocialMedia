namespace SocialMedia.Core.Interfaces.User;
public interface IUserRepository
{
    public Task<IEnumerable<UserDTO>> GetAsync();
    public Task<UserWithPostsAndCommentsDTO> GetByIdAsync(int id);
    public Task<UserDTO?> PostAsync(CreateUserDTO post);
    public Task<UserDTO?> UpdateAsync(CreateUserDTO post, int id);
    public Task<bool> DeleteAsync(int id);
}
