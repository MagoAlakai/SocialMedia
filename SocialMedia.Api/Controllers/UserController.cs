using SocialMedia.Core.DTOs.Users;

namespace SocialMedia.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    //POST https://localhost:7022/api/user
    [HttpPost]
    public async Task<IActionResult> Post(CreateUserDTO create_user_dto)
    {
        try
        {
            UserDTO? result = await _userRepository.PostAsync(create_user_dto);
            if (result is null) { return BadRequest($"This user is already registered"); }

            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex.Message));
        }
    }
}
