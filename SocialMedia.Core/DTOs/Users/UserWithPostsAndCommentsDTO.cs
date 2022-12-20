namespace SocialMedia.Core.DTOs.Users;

public class UserWithPostsAndCommentsDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public DateTime BirthDate { get; set; }
    public string? PhoneNumber { get; set; }
    public bool Active { get; set; }
    public virtual ICollection<CommentSimplifiedDTO>? Comments { get; set; }
    public virtual ICollection<PostSimplifiedDTO>? Posts { get; set; }
}
