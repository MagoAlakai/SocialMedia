namespace SocialMedia.Core.DTOs.Posts;

public class PostWithCommentsDTO : PostDTO
{
    public ICollection<Comment>? Comments { get; set; }
}
