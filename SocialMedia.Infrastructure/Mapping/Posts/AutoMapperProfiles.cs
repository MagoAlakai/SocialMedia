namespace SocialMedia.Infrastructure.Mapping.Posts;
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<CreatePostDTO, Post>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<CreatePostDTO, PostDTO>();
        CreateMap<PostDTO, CreatePostDTO>();
        CreateMap<Post, CreatePostDTO>();
        CreateMap<Post, PostDTO>();
        CreateMap<PostDTO, Post>();
        CreateMap<Post, PostWithUserAndCommentsDTO>();
        CreateMap<Post, PostSimplifiedDTO>();
    }
}
