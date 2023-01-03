using SocialMedia.Core.Entities;

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

    public async Task<ValidatedResult<Comment>> GetByIdAsync(int id)
    {
        Comment? comment = await _unitOfWork.commentRepository.GetByIdAsync(id);
        if (comment is null) { return ValidatedResult<Comment>.Failed(0, "This Comment is not registered"); }

        Post? post = await _unitOfWork.postRepository.GetByIdAsync(comment.PostId);
        if (post is null) { return ValidatedResult<Comment>.Failed(0, "This Comment has no Post associated"); }

        User? user = await _unitOfWork.userRepository.GetByIdAsync(comment.UserId);
        if (user is null) { return ValidatedResult<Comment>.Failed(0, "This Comment has no User associated"); }

        comment.User = user;
        comment.Post = post;

        return ValidatedResult<Comment>.Passed(comment);
    }

    public async Task<ValidatedResult<Comment>> PostAsync(Comment create_comment)
    {
        Comment? comment = await _unitOfWork.commentRepository.PostAsync(create_comment);
        if (comment is null) { return ValidatedResult<Comment>.Failed(0, "This Comment is not registered"); }

        await _unitOfWork.SaveChangesAsync();

        return ValidatedResult<Comment>.Passed(comment); ;
    }

    public async Task<ValidatedResult<Comment>> UpdateAsync(Comment update_comment, int id)
    {
        Comment? comment = await _unitOfWork.commentRepository.UpdateAsync(update_comment, id);
        if (comment is null) { return ValidatedResult<Comment>.Failed(0, "This Comment is not registered"); }

        await _unitOfWork.SaveChangesAsync();

        return ValidatedResult<Comment>.Passed(comment);
    }

    public async Task<ValidatedResult<bool>> DeleteAsync(int id)
    {
        bool deleted = await _unitOfWork.commentRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return ValidatedResult<bool>.Passed(deleted); ;
    }
}
