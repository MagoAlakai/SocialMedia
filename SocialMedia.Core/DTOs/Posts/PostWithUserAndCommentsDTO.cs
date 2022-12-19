namespace SocialMedia.Core.DTOs.Posts;

public class PostWithUserAndCommentsDTO : PostDTO
{
    public UserDTO? User { get; set; }
    public ICollection<Comment>? Comments { get; set; }
}
