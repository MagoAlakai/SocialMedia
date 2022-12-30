namespace SocialMedia.Core.DTOs.Posts;

/// <summary>
/// DTO to display at the User or Comment in a simplified way
/// </summary>
public class PostSimplifiedDTO
{
    public int Id { get; set; }
    public string? Description { get; set; }
}
