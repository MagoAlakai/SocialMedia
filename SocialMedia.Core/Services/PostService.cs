﻿namespace SocialMedia.Core.Services;

public class PostService : IPostService
{
    private readonly IUnitOfWork _unitOfWork;

    public PostService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PostDTO>> GetAsync()
    {
        return await _unitOfWork.postRepository.GetAsync();
    }

    public async Task<PostWithUserAndCommentsDTO> GetByIdAsync(int id)
    {
        return await _unitOfWork.postRepository.GetByIdAsync(id);
    }

    public async Task<PostDTO?> PostAsync(CreatePostDTO create_post_dto)
    {
        var user = await _unitOfWork.userRepository.GetByIdAsync(create_post_dto.UserId);
        if (user.Id is 0)
        {
            throw new Exception("User does not exist");
        }

        return await _unitOfWork.postRepository.PostAsync(create_post_dto);
    }

    public async Task<PostDTO?> UpdateAsync(CreatePostDTO create_post_dto, int id)
    {
        return await _unitOfWork.postRepository.UpdateAsync(create_post_dto, id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _unitOfWork.postRepository.DeleteAsync(id);
    }
}