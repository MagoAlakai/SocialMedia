namespace SocialMedia.Core.Interfaces.Identity;

public interface IIdentityService
{
    public Task<ValidatedResult<AuthenticationResponseDTO>> BuildToken(UserCredentialsDTO user_credentials_dto);
}
