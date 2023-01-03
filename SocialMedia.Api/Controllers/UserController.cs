namespace SocialMedia.Api.Controllers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IValidator<CreateUserDTO> _validator;
    private readonly IMapper _mapper;

    public UserController(IUserService userService, IValidator<CreateUserDTO> validator, IMapper mapper)
    {
        _userService = userService;
        _validator = validator;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all the Users
    /// </summary>
    /// <remarks>GET https://localhost:7022/api/user</remarks>
    /// <returns>A list of Users</returns>
    [HttpGet]
    public async Task<ActionResult<ValidatedResult<IEnumerable<UserDTO>>>> Get()
    {
        try
        {
            ValidatedResult<IEnumerable<User>> result = await _userService.GetAsync();
            if (result.Success is false || result.Value is null) { return ValidatedResult<IEnumerable<UserDTO>>.Failed(0, $"There are no users registered yet"); }

            List<UserDTO> user_dto_list = new(_mapper.Map<IEnumerable<UserDTO>>(result.Value));

            return StatusCode(200, ValidatedResult<IEnumerable<UserDTO>>.Passed(user_dto_list));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidatedResult<IEnumerable<UserDTO>>.Failed(0, ex.Message));
        }
    }

    /// <summary>
    /// Get details from a User, with Posts and Comments
    /// </summary>
    /// <param name="id"></param>
    /// <remarks>GET https://localhost:7022/api/user/{id}</remarks>
    /// <returns>A UserWithPostsAndCommentsDTO</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ValidatedResult<UserWithPostsAndCommentsDTO>>> GetById(int id)
    {
        try
        {
            ValidatedResult<User> result = await _userService.GetByIdAsync(id);
            if (result.Success is false || result.Value is null) { return ValidatedResult<UserWithPostsAndCommentsDTO>.Failed(0, "This user is not registered"); }

            List<CommentSimplifiedDTO> comments_dto = _mapper.Map<List<CommentSimplifiedDTO>>(result.Value.Comments);

            List<PostSimplifiedDTO> posts_dto = _mapper.Map<List<PostSimplifiedDTO>>(result.Value.Posts);

            UserWithPostsAndCommentsDTO user_with_posts_and_comments_dto = _mapper.Map<User, UserWithPostsAndCommentsDTO>(result.Value, options =>
                   options.AfterMap((src, dest) =>
                   {
                       dest.Posts = posts_dto;
                       dest.Comments = comments_dto;
                   }));

            return StatusCode(200, ValidatedResult<UserWithPostsAndCommentsDTO>.Passed(user_with_posts_and_comments_dto));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidatedResult<UserWithPostsAndCommentsDTO>.Failed(0, ex.Message));
        }
    }

    /// <summary>
    /// Create a User
    /// </summary>
    /// <param name="create_user_dto"></param>
    /// <remarks>POST https://localhost:7022/api/user</remarks>
    /// <returns>A StatusCode and the UserDTO</returns>
    [HttpPost]
    public async Task<ActionResult<ValidatedResult<UserDTO>>> Post(CreateUserDTO create_user_dto)
    {
        try
        {
            ValidationResult validation = await _validator.ValidateAsync(create_user_dto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            User? user = _mapper.Map<User>(create_user_dto);
            ValidatedResult<User> result = await _userService.PostAsync(user);
            if (result.Success is false || result.Value is null) { return ValidatedResult<UserDTO>.Failed(0, "This user is already registered"); }

            UserDTO? user_dto = _mapper.Map<UserDTO>(result.Value);

            return StatusCode(200, ValidatedResult<UserDTO>.Passed(user_dto));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidatedResult<UserDTO>.Failed(0, ex.Message));
        }
    }

    /// <summary>
    /// Update a User
    /// </summary>
    /// <param name="update_user_dto"></param>
    /// <param name="id"></param>
    /// <remarks>PUT https://localhost:7022/api/user/{id}</remarks>
    /// <returns>A StatusCode and the UserDTO</returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ValidatedResult<UserDTO>>> Update(CreateUserDTO update_user_dto, int id)
    {
        try
        {
            ValidationResult validation = await _validator.ValidateAsync(update_user_dto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            User? user = _mapper.Map<User>(update_user_dto);
            ValidatedResult<User> result = await _userService.UpdateAsync(user, id);
            if (result.Success is false || result.Value is null) { return ValidatedResult<UserDTO>.Failed(0, "This user is not registered"); }

            UserDTO? user_dto = _mapper.Map<UserDTO>(result.Value);

            return StatusCode(200, ValidatedResult<UserDTO>.Passed(user_dto));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidatedResult<UserDTO>.Failed(0, ex.Message));
        }
    }

    /// <summary>
    /// Delete a User
    /// </summary>
    /// <param name="id"></param>
    /// <remarks>DELETE https://localhost:7022/api/user/{id}</remarks>
    /// <returns>A boolean</returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ValidatedResult<bool>>> Delete(int id)
    {
        try
        {
            ValidatedResult<bool> result = await _userService.DeleteAsync(id);
            if (result.Success is false || result.Value is false) { return ValidatedResult<bool>.Failed(0, "This user is not registered"); }

            return StatusCode(200, ValidatedResult<bool>.Passed(result.Value));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidatedResult<bool>.Failed(0, ex.Message));
        }
    }
}
