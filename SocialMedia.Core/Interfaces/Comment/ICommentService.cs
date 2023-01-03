namespace SocialMedia.Core.Interfaces.Comment;

public interface ICommentService
{
    public Task<ValidatedResult<IEnumerable<Entities.Comment>>> GetAsync();
    public Task<ValidatedResult<Entities.Comment>> GetByIdAsync(int id);
    public Task<ValidatedResult<Entities.Comment>> PostAsync(Entities.Comment create_comment);
    public Task<ValidatedResult<Entities.Comment>> UpdateAsync(Entities.Comment update_comment, int id);
    public Task<ValidatedResult<bool>> DeleteAsync(int id);
}
