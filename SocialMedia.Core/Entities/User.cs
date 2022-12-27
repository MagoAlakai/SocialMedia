namespace SocialMedia.Core.Entities;

public class User : BaseEntity
{
    public User()
    {
        Comments = new HashSet<Comment>();
        Posts = new HashSet<Post>();
    }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public DateTime BirthDate { get; set; }
    public string? PhoneNumber { get; set; }
    public bool Active { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<Post> Posts { get; set; }

}
