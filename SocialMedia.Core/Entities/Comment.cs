namespace SocialMedia.Core.Entities;

public class Comment
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public bool Active { get; set; }
    public virtual Post? PostIdNavigation { get; set; }
    public virtual User? UserIdNavigation { get; set; }
}
