namespace SocialMedia.Core.DTOs.Comments;

public class CreateCommentDTO
{
    public string? Description { get; set; }
    public bool Active { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}
