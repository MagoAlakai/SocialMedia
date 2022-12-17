
using Microsoft.EntityFrameworkCore;
using SocialMedia.Infrastructure.Data;


namespace SocialMedia.Infrastructure.Repositories.Posts;

public class PostRepository : IPostRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public PostRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PostDTO>> GetAsync()
    {
        IEnumerable<Post> posts = await _applicationDbContext.Posts.ToListAsync();
        if (posts is null) { return new List<PostDTO>(); }

        List<PostDTO> post_dto_list = new(_mapper.Map<IEnumerable<PostDTO>>(posts));

        return await Task.FromResult(post_dto_list);
    }

    public async Task<PostDTO> GetByIdAsync(int id)
    {
        Post? post = await _applicationDbContext.Posts.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (post is null) { return new PostDTO(); }

        PostDTO post_dto = _mapper.Map<PostDTO>(post);

        return await Task.FromResult(post_dto);
    }

    public async Task<PostDTO?> PostAsync(CreatePostDTO create_post_dto)
    {
        bool post_exist = await _applicationDbContext.Posts.AnyAsync(x => x.Description == create_post_dto.Description);
        if (post_exist is true) { return null; }

        Post? post = _mapper.Map<Post>(create_post_dto);

        _applicationDbContext.Add(post);
        await _applicationDbContext.SaveChangesAsync();

        PostDTO? post_dto = _mapper.Map<PostDTO>(post);

        return post_dto;
    }

    public async Task<PostDTO?> UpdateAsync(CreatePostDTO create_post_dto, int id)
    {
        bool post_exist = await _applicationDbContext.Posts.AnyAsync(x => x.Id == id);
        if (post_exist is false) { return null; }

        Post? post = _mapper.Map<CreatePostDTO, Post>(create_post_dto, options =>
        options.AfterMap((src, dest) =>
        {
            dest.Id = id;
        }));

        _applicationDbContext.Update(post);
        await _applicationDbContext.SaveChangesAsync();

        PostDTO? post_dto = _mapper.Map<PostDTO>(post);

        return post_dto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Post? post = await _applicationDbContext.Posts.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (post is null) { return false; }

        _applicationDbContext.Remove(post);
        await _applicationDbContext.SaveChangesAsync();

        return true;
    }
}
