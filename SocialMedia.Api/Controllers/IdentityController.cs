namespace SocialMedia.Api.Controllers;

[Route("api/identity")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly UserManager<IdentityUser> _user_manager;
    private readonly SignInManager<IdentityUser> _signin_manager;
    private readonly IDataProtector _data_protector;
    private readonly IIdentityService _identity_service;

    public IdentityController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signinManager, IIdentityService identityService, IDataProtectionProvider idata_protection_provider)
    {
        _user_manager = userManager;
        _signin_manager = signinManager;
        _identity_service = identityService;
        _data_protector = idata_protection_provider.CreateProtector("unique_and_secret_value");
    }

    /// <summary>
    /// Register an IdentityUser
    /// </summary>
    /// <param name="user_credentials_dto"></param>
    /// <remarks>POST https://localhost:7022/api/identity/register</remarks>
    /// <returns>A ValidatedResult of AuthenticationResponseDTO</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<ValidatedResult<AuthenticationResponseDTO>>> Register(UserCredentialsDTO user_credentials_dto)
    {
        IdentityUser identity_user = new() { UserName = user_credentials_dto.Email, Email = user_credentials_dto.Email };
        IdentityResult identity_result = await _user_manager.CreateAsync(identity_user, user_credentials_dto.Password);

        if (identity_result.Succeeded is false) { return BadRequest(identity_result.Errors); };

        ValidatedResult<AuthenticationResponseDTO> authentication_response_dto_result = await _identity_service.BuildToken(user_credentials_dto);
        if (authentication_response_dto_result.Success is false || authentication_response_dto_result.Value is null)
        {
            return UnprocessableEntity(authentication_response_dto_result.Code);
        }

        authentication_response_dto_result.Value.UserCredentialsDTO = user_credentials_dto;
        authentication_response_dto_result.Value.SetSuccessToTrue();
        return authentication_response_dto_result;
    }

    /// <summary>
    /// Login an IdentityUser
    /// </summary>
    /// <param name="user_credentials_dto"></param>
    /// <remarks>POST https://localhost:7022/api/identity/login</remarks>
    /// <returns>A ValidatedResult of AuthenticationResponseDTO</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ValidatedResult<AuthenticationResponseDTO>>> Login(UserCredentialsDTO user_credentials_dto)
    {
        Microsoft.AspNetCore.Identity.SignInResult login_result = await _signin_manager.PasswordSignInAsync(
            user_credentials_dto.Email,
            user_credentials_dto.Password,
            isPersistent: false,
            lockoutOnFailure: false
        );

        if (login_result.Succeeded is false) { return BadRequest("The credentials are incorrect"); };

        ValidatedResult<AuthenticationResponseDTO> authentication_response_dto_result = await _identity_service.BuildToken(user_credentials_dto);
        if (authentication_response_dto_result.Success is false || authentication_response_dto_result.Value is null)
        {
            return UnprocessableEntity(authentication_response_dto_result.Code);
        }

        authentication_response_dto_result.Value.SetSuccessToTrue();
        return authentication_response_dto_result;
    }

    /// <summary>
    /// Refresh the login token
    /// </summary>
    /// <remarks>GET https://localhost:7022/api/identity/refreshToken</remarks>
    /// <returns></returns>
    [HttpGet("refreshToken")]
    public async Task<ActionResult<AuthenticationResponseDTO>> RefreshToken()
    {
        Claim? emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(emailClaim?.Value) is true)
        {
            return UnprocessableEntity();
        }

        string email = emailClaim.Value;
        UserCredentialsDTO user_credentials_dto = new() { Email = email, Password = string.Empty };

        ValidatedResult<AuthenticationResponseDTO> authentication_response_dto_result = await _identity_service.BuildToken(user_credentials_dto);
        if (authentication_response_dto_result.Success is false || authentication_response_dto_result.Value is null)
        {
            return UnprocessableEntity(authentication_response_dto_result.Code);
        }

        authentication_response_dto_result.Value.SetSuccessToTrue();
        return authentication_response_dto_result.Value;
    }

    /// <summary>
    /// Add Admin Claim to IdentityUser
    /// </summary>
    /// <param name="edit_admin_dto"></param>
    /// <remarks>POST https://localhost:7022/api/identity/makeAdmin</remarks>
    /// <returns>StatusCode</returns>
    [HttpPost("makeAdmin")]
    public async Task<ActionResult> MakeAdmin(EditAdminDTO edit_admin_dto)
    {
        IdentityUser? user = await _user_manager.FindByEmailAsync(edit_admin_dto.Email);
        if (user is null)
        {
            return UnprocessableEntity();
        }

        await _user_manager.AddClaimAsync(user, new Claim("isAdmin", "1"));
        return Ok();
    }

    /// <summary>
    /// Remove Admin Claim from IdentityUser
    /// </summary>
    /// <param name="edit_admin_dto"></param>
    /// <remarks>POST https://localhost:7022/api/identity/removeAdmin</remarks>
    /// <returns></returns>
    [HttpPost("removeAdmin")]
    public async Task<ActionResult> RemoveAdmin(EditAdminDTO edit_admin_dto)
    {
        IdentityUser? user = await _user_manager.FindByEmailAsync(edit_admin_dto.Email);
        if (user is null)
        {
            return UnprocessableEntity();
        }

        await _user_manager.RemoveClaimAsync(user, new Claim("isAdmin", "1"));
        return Ok();
    }
}
