using SocialMedia.Core.Interfaces.Post;
using SocialMedia.Core.Interfaces.UnitOfWork;
using SocialMedia.Core.Interfaces.User;

namespace SocialMedia.Infrastructure.Repositories.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    private readonly IPostRepository? _postRepository;
    private readonly IUserRepository? _userRepository;

    public UnitOfWork(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public IPostRepository postRepository => _postRepository ?? new PostRepository(_applicationDbContext, _mapper);
    public IUserRepository userRepository => _userRepository ?? new UserRepository(_applicationDbContext, _mapper);

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
