namespace SocialMedia.Infrastructure.Mapping.Comments;
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<CreateCommentDTO, Comment>();
        CreateMap<Comment, CreateCommentDTO>();
        CreateMap<Comment, CommentDTO>();
        CreateMap<CommentDTO, Comment>();
    }
}

