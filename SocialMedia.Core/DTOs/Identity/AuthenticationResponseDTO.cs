namespace SocialMedia.Core.DTOs.Identity;

/// <summary>
/// Gets the response of the authentication
/// </summary>
public class AuthenticationResponseDTO
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public DateTime ExpireDate { get; set; }
    public UserCredentialsDTO? UserCredentialsDTO { get; set; }

    /// <summary>
    /// All this does is set the <see cref="Success"/> value to true
    /// </summary>
    public void SetSuccessToTrue() => Success = true;
}
