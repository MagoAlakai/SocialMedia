namespace SocialMedia.Core.Entities;

public class User
{
    public User()
    {
        Comment = new HashSet<Comment>();
        Post = new HashSet<Post>();
    }

    public int Id { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public DateTime BirthDate { get; set; }
    public string? PhoneNumber { get; set; }
    public bool Active { get; set; }
    public virtual ICollection<Comment> Comment { get; set; }
    public virtual ICollection<Post> Post { get; set; }

}
