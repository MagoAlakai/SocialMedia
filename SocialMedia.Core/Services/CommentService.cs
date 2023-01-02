namespace SocialMedia.Core.Services;

public class CommentService : ICommentService
{
    private readonly IUnitOfWork _unitOfWork;

    public CommentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ValidatedResult<IEnumerable<Comment>>> GetAsync()
    {
        IEnumerable<Comment>? result = await _unitOfWork.commentRepository.GetAsync();
        if (result is null) { return ValidatedResult<IEnumerable<Comment>>.Failed(0, "There are no Comments registered"); }

        return ValidatedResult<IEnumerable<Comment>>.Passed(result);
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        Comment? comment = await _unitOfWork.commentRepository.GetByIdAsync(id);
        if (comment is null) { return new Comment(); }

        Post? post = await _unitOfWork.postRepository.GetByIdAsync(comment.PostId);
        if (post is null) { return new Comment(); }

        User? user = await _unitOfWork.userRepository.GetByIdAsync(comment.UserId);
        if (user is null) { return new Comment(); }

        comment.User = user;
        comment.Post = post;

        return comment;
    }

    public async Task<Comment?> PostAsync(Comment create_comment)
    {
        Comment? comment = await _unitOfWork.commentRepository.PostAsync(create_comment);
        if (comment is null) { return null; }

        await _unitOfWork.SaveChangesAsync();

        return comment;
    }

    public async Task<Comment?> UpdateAsync(Comment update_comment, int id)
    {
        Comment? comment = await _unitOfWork.commentRepository.UpdateAsync(update_comment, id);
        if (comment is null) { return null; }

        await _unitOfWork.SaveChangesAsync();

        return comment;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        bool deleted = await _unitOfWork.commentRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return deleted;
    }
}
