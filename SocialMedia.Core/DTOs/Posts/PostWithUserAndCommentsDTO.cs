namespace SocialMedia.Core.DTOs.Posts;

public class PostWithUserAndCommentsDTO : PostDTO
{
    public UserSimplifiedDTO? User { get; set; }
    public ICollection<CommentSimplifiedDTO>? Comments { get; set; }
}
