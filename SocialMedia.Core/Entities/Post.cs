namespace SocialMedia.Core.Entities;

public class Post
{
    public Post()
    {
        Comment = new HashSet<Comment>();
    }

    public int Id { get; set; }
    //public int UserId { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    //public virtual User? UserIdNavigation { get; set; }
    public virtual ICollection<Comment> Comment { get; set; }
}
