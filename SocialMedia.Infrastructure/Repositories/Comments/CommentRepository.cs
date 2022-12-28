using SocialMedia.Core.DTOs.Comments;
using SocialMedia.Core.DTOs.Posts;
using SocialMedia.Core.Entities;

namespace SocialMedia.Infrastructure.Repositories.Comments;
public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public CommentRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CommentDTO>> GetAsync()
    {
        IEnumerable<Comment> comments = await _applicationDbContext.Comments.ToListAsync();
        if (comments is null) { return new List<CommentDTO>(); }

        List<CommentDTO> comment_dto_list = new(_mapper.Map<IEnumerable<CommentDTO>>(comments));

        return await Task.FromResult(comment_dto_list);
    }

    public async Task<CommentWithUserAndPostDTO> GetByIdAsync(int id)
    {
        Comment? comment = await _applicationDbContext.Comments.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (comment is null) { return new CommentWithUserAndPostDTO(); }

        Post? post = await _applicationDbContext.Posts.Where(x => x.Id == comment.PostId).FirstOrDefaultAsync();
        if (post is null) { return new CommentWithUserAndPostDTO(); }

        User? user = await _applicationDbContext.Users.Where(x => x.Id == comment.UserId).FirstOrDefaultAsync();
        if (user is null) { return new CommentWithUserAndPostDTO(); }

        UserSimplifiedDTO user_dto = _mapper.Map<UserSimplifiedDTO>(user);
        PostSimplifiedDTO post_dto = _mapper.Map<PostSimplifiedDTO>(post);

        CommentWithUserAndPostDTO comment_with_user_and_post_dto = _mapper.Map<Comment, CommentWithUserAndPostDTO>(comment, options =>
               options.AfterMap((src, dest) => {
                   dest.User = user_dto;
                   dest.Post = post_dto;
               }));

        return await Task.FromResult(comment_with_user_and_post_dto);
    }

    public async Task<CommentDTO?> PostAsync(CreateCommentDTO create_comment_dto)
    {
        bool comment_exist = await _applicationDbContext.Comments.AnyAsync(x => x.Description == create_comment_dto.Description);
        if (comment_exist is true) { return null; }

        User? user = await _applicationDbContext.Users.Where(x => x.Id == create_comment_dto.UserId).FirstOrDefaultAsync();
        Post? post = await _applicationDbContext.Posts.Where(x => x.Id == create_comment_dto.PostId).FirstOrDefaultAsync();

        Comment? comment = _mapper.Map<CreateCommentDTO, Comment>(create_comment_dto, options =>
        {
            options.AfterMap((src, dest) =>
            {
                dest.User = user;
                dest.Post = post;
            });
        });

        _applicationDbContext.Add(comment);
        await _applicationDbContext.SaveChangesAsync();

        CommentDTO? comment_dto = _mapper.Map<CommentDTO>(comment);

        return comment_dto;
    }

    public async Task<CommentDTO?> UpdateAsync(CreateCommentDTO update_comment_dto, int id)
    {
        bool comment_exist = await _applicationDbContext.Comments.AnyAsync(x => x.Id == id);
        if (comment_exist is true) { return null; }

        Comment? comment = _mapper.Map<CreateCommentDTO, Comment>(update_comment_dto, options =>
        {
            options.AfterMap((src, dest) =>
            {
                dest.Id = id;
            });
        });

        _applicationDbContext.Update(comment);
        await _applicationDbContext.SaveChangesAsync();

        CommentDTO? comment_dto = _mapper.Map<CommentDTO>(comment);

        return comment_dto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Comment? comment = await _applicationDbContext.Comments.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (comment is null) { return false; }

        _applicationDbContext.Remove(comment);
        await _applicationDbContext.SaveChangesAsync();

        return true;
    }
}
