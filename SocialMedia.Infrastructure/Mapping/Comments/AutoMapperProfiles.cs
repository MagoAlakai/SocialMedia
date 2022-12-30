namespace SocialMedia.Infrastructure.Mapping.Comments;
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<CreateCommentDTO, Comment>();
        CreateMap<Comment, CreateCommentDTO>();
        CreateMap<Comment, CommentWithUserAndPostDTO>();
        CreateMap<CommentWithUserAndPostDTO, Comment>();
        CreateMap<Comment, CommentSimplifiedDTO>();
        CreateMap<Comment, CommentDTO>();
    }
}

