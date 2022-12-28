using SocialMedia.Core.DTOs.Users;
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
        UserDTO? user_dto = await _unitOfWork.userRepository.PostAsync(create_user_dto);
        await _unitOfWork.SaveChangesAsync();

        return user_dto;
    }

    public async Task<UserDTO?> UpdateAsync(CreateUserDTO create_user_dto, int id)
    {
        UserDTO? user_dto = await _unitOfWork.userRepository.UpdateAsync(create_user_dto, id);
        await _unitOfWork.SaveChangesAsync();

        return user_dto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        bool deleted = await _unitOfWork.userRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return deleted;
    }
}
