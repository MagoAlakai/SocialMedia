namespace SocialMedia.Core.DTOs.Identity;

/// <summary>
/// Creates the credentials to Login
/// </summary>
public sealed class UserCredentialsDTO
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
