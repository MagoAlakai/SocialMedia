namespace SocialMedia.Core.DTOs.Comments;

public class CommentDTO
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public bool Active { get; set; }
}
