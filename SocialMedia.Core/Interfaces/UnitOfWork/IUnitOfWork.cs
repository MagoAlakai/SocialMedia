using SocialMedia.Core.Entities;
namespace SocialMedia.Core.Interfaces.UnitOfWork;
public interface IUnitOfWork : IDisposable
{
    IRepository<Entities.Post> postRepository { get; }
    IRepository<Entities.User> userRepository { get; }
    IRepository<Entities.Comment> commentRepository { get; }

    void SaveChanges();
    Task SaveChangesAsync();
}
