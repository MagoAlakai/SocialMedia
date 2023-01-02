namespace SocialMedia.Core.Interfaces.Comment;

public interface ICommentService
{
    public Task<ValidatedResult<IEnumerable<Entities.Comment>>> GetAsync();
    public Task<Entities.Comment?> GetByIdAsync(int id);
    public Task<Entities.Comment?> PostAsync(Entities.Comment create_comment);
    public Task<Entities.Comment?> UpdateAsync(Entities.Comment update_comment, int id);
    public Task<bool> DeleteAsync(int id);
}
