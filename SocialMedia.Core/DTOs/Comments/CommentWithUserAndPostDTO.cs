namespace SocialMedia.Core.DTOs.Comments;

/// <summary>
/// DTO to display the Comment with User and Post
/// </summary>
public class CommentWithUserAndPostDTO : CommentDTO
{
    public PostSimplifiedDTO? Post { get; set; }
    public UserSimplifiedDTO? User { get; set; }
}
