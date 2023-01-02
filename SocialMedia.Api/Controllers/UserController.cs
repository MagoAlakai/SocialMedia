using SocialMedia.Core.Data;
using System.Collections.Generic;

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
            if (result.Success is false) { return BadRequest($"There are no users registered yet"); }

            List<UserDTO> user_dto_list = new(_mapper.Map<IEnumerable<UserDTO>>(result));

            return StatusCode(200, ValidatedResult<IEnumerable<UserDTO>>.Passed(user_dto_list));
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex));
        }
    }

    /// <summary>
    /// Get details from a User, with Posts and Comments
    /// </summary>
    /// <param name="id"></param>
    /// <remarks>GET https://localhost:7022/api/user/{id}</remarks>
    /// <returns>A UserWithPostsAndCommentsDTO</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserWithPostsAndCommentsDTO>> GetById(int id)
    {
        try
        {
            User? result = await _userService.GetByIdAsync(id);
            if (result is null) { return BadRequest($"This user is not registered"); }

            List<CommentSimplifiedDTO> comments_dto = _mapper.Map<List<CommentSimplifiedDTO>>(result.Comments);

            List<PostSimplifiedDTO> posts_dto = _mapper.Map<List<PostSimplifiedDTO>>(result.Posts);

            UserWithPostsAndCommentsDTO user_with_posts_and_comments_dto = _mapper.Map<User, UserWithPostsAndCommentsDTO>(result, options =>
                   options.AfterMap((src, dest) =>
                   {
                       dest.Posts = posts_dto;
                       dest.Comments = comments_dto;
                   }));

            return StatusCode(200, user_with_posts_and_comments_dto);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex));
        }
    }

    /// <summary>
    /// Create a User
    /// </summary>
    /// <param name="create_user_dto"></param>
    /// <remarks>POST https://localhost:7022/api/user</remarks>
    /// <returns>A StatusCode and the UserDTO</returns>
    [HttpPost]
    public async Task<ActionResult<UserDTO>> Post(CreateUserDTO create_user_dto)
    {
        try
        {
            ValidationResult validation = await _validator.ValidateAsync(create_user_dto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            User? user = _mapper.Map<User>(create_user_dto);
            User? result = await _userService.PostAsync(user);
            if (result is null) { return BadRequest($"This user is already registered"); }

            UserDTO? user_dto = _mapper.Map<UserDTO>(result);

            return StatusCode(200, user_dto);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex));
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
    public async Task<ActionResult<UserDTO>> Update(CreateUserDTO update_user_dto, int id)
    {
        try
        {
            ValidationResult validation = await _validator.ValidateAsync(update_user_dto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            User? user = _mapper.Map<User>(update_user_dto);
            User? result = await _userService.UpdateAsync(user, id);
            if (result is null) { return BadRequest($"This user is not registered"); }

            UserDTO? user_dto = _mapper.Map<UserDTO>(result);

            return StatusCode(200, user_dto);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex));
        }
    }

    /// <summary>
    /// Delete a User
    /// </summary>
    /// <param name="id"></param>
    /// <remarks>DELETE https://localhost:7022/api/user/{id}</remarks>
    /// <returns>A boolean</returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        try
        {
            bool result = await _userService.DeleteAsync(id);
            if (result is false) { return BadRequest($"This user is not registered"); }

            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex));
        }
    }
}
