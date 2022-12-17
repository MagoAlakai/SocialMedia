namespace SocialMedia.Core.DTOs.Users;

public class UserDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public DateTime BirthDate { get; set; }
    public string? PhoneNumber { get; set; }
    public bool Active { get; set; }
}
