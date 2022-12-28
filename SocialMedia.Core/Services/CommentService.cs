using SocialMedia.Core.DTOs.Comments;

namespace SocialMedia.Core.Services;

public class CommentService : ICommentService
{
    private readonly IUnitOfWork _unitOfWork;

    public CommentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<CommentDTO>> GetAsync()
    {
        return await _unitOfWork.commentRepository.GetAsync();
    }

    public async Task<CommentWithUserAndPostDTO> GetByIdAsync(int id)
    {
        return await _unitOfWork.commentRepository.GetByIdAsync(id);
    }

    public async Task<CommentDTO?> PostAsync(CreateCommentDTO post)
    {
        CommentDTO? comment_dto = await _unitOfWork.commentRepository.PostAsync(post);
        await _unitOfWork.SaveChangesAsync();

        return comment_dto;
    }

    public async Task<CommentDTO?> UpdateAsync(CreateCommentDTO post, int id)
    {
        CommentDTO? comment_dto = await _unitOfWork.commentRepository.UpdateAsync(post, id);
        await _unitOfWork.SaveChangesAsync();

        return comment_dto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        bool deleted = await _unitOfWork.commentRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return deleted;
    }
}
