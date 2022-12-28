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
        return await _unitOfWork.commentRepository.PostAsync(post);
    }

    public async Task<CommentDTO?> UpdateAsync(CreateCommentDTO post, int id)
    {
        return await _unitOfWork.commentRepository.UpdateAsync(post, id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _unitOfWork.commentRepository.DeleteAsync(id);
    }
}
