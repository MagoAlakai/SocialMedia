using SocialMedia.Core.Interfaces.User;

namespace SocialMedia.Core.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UserDTO>> GetAsync()
    {
        return await _unitOfWork.userRepository.GetAsync();
    }

    public async Task<UserWithPostsAndCommentsDTO> GetByIdAsync(int id)
    {
        return await _unitOfWork.userRepository.GetByIdAsync(id);
    }

    public async Task<UserDTO?> PostAsync(CreateUserDTO create_user_dto)
    {
        return await _unitOfWork.userRepository.PostAsync(create_user_dto);
    }

    public async Task<UserDTO?> UpdateAsync(CreateUserDTO create_user_dto, int id)
    {
        return await _unitOfWork.userRepository.UpdateAsync(create_user_dto, id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _unitOfWork.userRepository.DeleteAsync(id);
    }
}
