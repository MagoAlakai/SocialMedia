namespace SocialMedia.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<CreateUserDTO> _validator;

    public UserController(IUserRepository userRepository, IValidator<CreateUserDTO> validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }

    //GET https://localhost:7022/api/user
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            IEnumerable<UserDTO> result = await _userRepository.GetAsync();
            if (result is null) { return BadRequest($"There are no users registered yet"); }

            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex.Message));
        }
    }

    //GET https://localhost:7022/api/user/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            UserWithPostsAndCommentsDTO result = await _userRepository.GetByIdAsync(id);
            if (result is null) { return BadRequest($"This user is not registered"); }

            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex.Message));
        }
    }

    //POST https://localhost:7022/api/user
    [HttpPost]
    public async Task<IActionResult> Post(CreateUserDTO create_user_dto)
    {
        try
        {
            ValidationResult validation = await _validator.ValidateAsync(create_user_dto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            UserDTO? result = await _userRepository.PostAsync(create_user_dto);
            if (result is null) { return BadRequest($"This user is already registered"); }

            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex.Message));
        }
    }

    //PUT https://localhost:7022/api/user/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(CreateUserDTO update_user_dto, int id)
    {
        try
        {
            ValidationResult validation = await _validator.ValidateAsync(update_user_dto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            UserDTO? result = await _userRepository.UpdateAsync(update_user_dto, id);
            if (result is null) { return BadRequest($"This user is not registered"); }

            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex.Message));
        }
    }

    //DELETE https://localhost:7022/api/user/{id}
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            bool result = await _userRepository.DeleteAsync(id);
            if (result is false) { return BadRequest($"This user is not registered"); }

            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex.Message));
        }
    }
}
