namespace SocialMedia.Infrastructure.Repositories.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IRepository<Post>? _postRepository;
    private readonly IRepository<User>? _userRepository;
    private readonly IRepository<Comment>? _commentRepository;
    public UnitOfWork(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public IRepository<Post> postRepository => _postRepository ?? new BaseRepository<Post>(_applicationDbContext);
    public IRepository<User> userRepository => _userRepository ?? new BaseRepository<User>(_applicationDbContext);
    public IRepository<Comment> commentRepository => _commentRepository ?? new BaseRepository<Comment>(_applicationDbContext);

    public void Dispose()
    {
        if (_applicationDbContext != null)
        {
            _applicationDbContext.Dispose();
        }
    }

    public void SaveChanges()
    {
        _applicationDbContext.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
        await _applicationDbContext.SaveChangesAsync();
    }
}
