namespace SocialMedia.Core.Interfaces.UnitOfWork;
public interface IUnitOfWork : IDisposable
{
    IPostRepository postRepository { get; }
    IUserRepository userRepository { get; }
    ICommentRepository commentRepository { get; }

    void SaveChanges();
    Task SaveChangesAsync();
}
