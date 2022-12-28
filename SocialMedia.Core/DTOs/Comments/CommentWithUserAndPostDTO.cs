namespace SocialMedia.Core.DTOs.Comments;

public class CommentWithUserAndPostDTO : CommentDTO
{
    public PostSimplifiedDTO? Post { get; set; }
    public UserSimplifiedDTO? User { get; set; }
}
