namespace SocialMedia.Core.Services;

public class IdentityService : IIdentityService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<IdentityService> _logger;
    private readonly UserManager<IdentityUser> _user_manager;

    public IdentityService(IConfiguration configuration, ILogger<IdentityService> logger, UserManager<IdentityUser> user_manager)
    {
        _configuration = configuration;
        _logger = logger;
        _user_manager = user_manager;
    }

    public async Task<ValidatedResult<AuthenticationResponseDTO>> BuildToken(UserCredentialsDTO user_credentials_dto)
    {
        List<Claim> claims = new()
        {
            new Claim("email", user_credentials_dto.Email)
        };

        IdentityUser? user = await _user_manager.FindByEmailAsync(user_credentials_dto.Email);

        if (user is null || user_credentials_dto.Email == string.Empty || user_credentials_dto.Password == string.Empty)
        {
            return ValidatedResult<AuthenticationResponseDTO>.Failed(0, "FindByEmailAsync returned null");
        }

        IList<Claim> claims_model = await _user_manager.GetClaimsAsync(user);
        claims.AddRange(claims_model);

        string? jwtkey = _configuration["Authentication:SecretKey"];
        if (string.IsNullOrEmpty(jwtkey)) { throw new ArgumentNullException("The secret key must be a string"); }

        byte[] jwtkey_byte_array = Encoding.UTF8.GetBytes(jwtkey);
        SymmetricSecurityKey register_jwt = new(jwtkey_byte_array);

        SigningCredentials creds = new(register_jwt, SecurityAlgorithms.HmacSha256);
        DateTime expire_date = DateTime.UtcNow.AddMinutes(30);

        JwtSecurityToken security_token = new(issuer: _configuration["Authentication:Issuer"], audience: _configuration["Authentication:Audience"], claims: claims, expires: expire_date, signingCredentials: creds);

        AuthenticationResponseDTO authentication_response_dto = new() { ExpireDate = expire_date, Token = new JwtSecurityTokenHandler().WriteToken(security_token), UserCredentialsDTO = user_credentials_dto };
        return ValidatedResult<AuthenticationResponseDTO>.Passed(authentication_response_dto);
    }
}
