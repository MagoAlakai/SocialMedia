namespace SocialMedia.Core.Interfaces.User;

public interface IUserService
{
    public Task<bool> DeleteAsync(int id);
    public Task<IEnumerable<UserDTO>> GetAsync();
    public Task<UserWithPostsAndCommentsDTO> GetByIdAsync(int id);
    public Task<UserDTO?> PostAsync(CreateUserDTO create_user_dto);
    public Task<UserDTO?> UpdateAsync(CreateUserDTO create_user_dto, int id);
}