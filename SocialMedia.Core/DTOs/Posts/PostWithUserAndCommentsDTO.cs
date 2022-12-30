namespace SocialMedia.Core.DTOs.Posts;

/// <summary>
/// DTO to display the Post with User and Comments
/// </summary>
public class PostWithUserAndCommentsDTO : PostDTO
{
    public UserSimplifiedDTO? User { get; set; }
    public ICollection<CommentSimplifiedDTO>? Comments { get; set; }
}
