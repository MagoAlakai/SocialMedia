namespace SocialMedia.Core.DTOs.Comments;

/// <summary>
/// DTO to display at the User or Post in a simplified way
/// </summary>
public class CommentSimplifiedDTO
{
    public int Id { get; set; }
    public string? Description { get; set; }
}
