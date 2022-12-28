namespace SocialMedia.Core.Interfaces.Comment;

public interface ICommentRepository
{
    public Task<IEnumerable<CommentDTO>> GetAsync();
    public Task<CommentWithUserAndPostDTO> GetByIdAsync(int id);
    public Task<CommentDTO?> PostAsync(CreateCommentDTO post);
    public Task<CommentDTO?> UpdateAsync(CreateCommentDTO post, int id);
    public Task<bool> DeleteAsync(int id);
}
