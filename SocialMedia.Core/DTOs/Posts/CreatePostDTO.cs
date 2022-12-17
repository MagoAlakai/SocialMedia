namespace SocialMedia.Core.DTOs.Posts;

public class CreatePostDTO
{
    public string? Description { get; set; }
    public string? Image { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}
