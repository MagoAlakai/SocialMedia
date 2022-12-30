namespace SocialMedia.Core.DTOs.Users;

/// <summary>
/// DTO to display at the Comment or Post in a simplified way
/// </summary>
public class UserSimplifiedDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
}