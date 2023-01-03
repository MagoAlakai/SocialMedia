using SocialMedia.Core.Data;

namespace SocialMedia.Api.Controllers;

[Route("api/comment")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;
    private readonly IValidator<CreateCommentDTO> _validator;
    private readonly IMapper _mapper;

    public CommentController(ICommentService commentService, IValidator<CreateCommentDTO> validator, IMapper mapper)
    {
        _commentService = commentService;
        _validator = validator;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieve all the comments
    /// </summary>
    /// <remarks>GET https://localhost:7022/api/comment</remarks>
    /// <returns>A list of Comments</returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ValidatedResult<IEnumerable<CommentDTO>>>> GetComments()
    {
        try
        {
            ValidatedResult<IEnumerable<Comment>> result = await _commentService.GetAsync();
            if (result.Success is false || result.Value is null) { return ValidatedResult<IEnumerable<CommentDTO>>.Failed(0, $"There are no comments registered yet"); }

            List<CommentDTO> comment_dto_list = new(_mapper.Map<IEnumerable<CommentDTO>>(result));

            return StatusCode(200, ValidatedResult<IEnumerable<CommentDTO>>.Passed(comment_dto_list));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidatedResult<IEnumerable<CommentDTO>>.Failed(0, ex.Message));
        }
    }

    /// <summary>
    /// Get details from specific Comment, with User and Posts
    /// </summary>
    /// <param name="id"></param>
    /// <remarks>GET https://localhost:7022/api/comment/{id}</remarks>
    /// <returns>A CommentWithUserAndPostDTO</returns>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<ValidatedResult<CommentWithUserAndPostDTO>>> GetById(int id)
    {
        try
        {
            ValidatedResult<Comment> result = await _commentService.GetByIdAsync(id);
            if (result.Success is false || result.Value is null) { return ValidatedResult<CommentWithUserAndPostDTO>.Failed(0, $"This comment is not registered"); }

            UserSimplifiedDTO user_dto = _mapper.Map<UserSimplifiedDTO>(result.Value.User);
            PostSimplifiedDTO post_dto = _mapper.Map<PostSimplifiedDTO>(result.Value.Post);

            CommentWithUserAndPostDTO comment_with_user_and_post_dto = _mapper.Map<Comment, CommentWithUserAndPostDTO>(result.Value, options =>
                   options.AfterMap((src, dest) => {
                       dest.User = user_dto;
                       dest.Post = post_dto;
                   }));

            return StatusCode(200, ValidatedResult<CommentWithUserAndPostDTO>.Passed(comment_with_user_and_post_dto));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidatedResult<CommentWithUserAndPostDTO>.Failed(0, ex.Message));
        }
    }

    /// <summary>
    /// Create a Comment
    /// </summary>
    /// <param name="create_comment_dto"></param>
    /// <remarks>POST https://localhost:7022/api/comment/</remarks>
    /// <returns>A StatusCode and a CommentDTO</returns>
    [HttpPost]
    public async Task<ActionResult<ValidatedResult<CommentDTO>>> Post(CreateCommentDTO create_comment_dto)
    {
        try
        {
            ValidationResult validation = await _validator.ValidateAsync(create_comment_dto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            Comment comment = _mapper.Map<Comment>(create_comment_dto);
            ValidatedResult<Comment> result = await _commentService.PostAsync(comment);
            if (result.Success is false || result.Value is null) { return ValidatedResult<CommentDTO>.Failed(0, $"This Comment is already registered"); }

            CommentDTO? comment_dto = _mapper.Map<CommentDTO>(result.Value);

            return StatusCode(200, ValidatedResult<CommentDTO>.Passed(comment_dto));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidatedResult<CommentDTO>.Failed(0, ex.Message));
        }
    }

    /// <summary>
    /// Update a Comment
    /// </summary>
    /// <param name="update_comment_dto"></param>
    /// <param name="id"></param>
    /// <remarks>PUT https://localhost:7022/api/comment/{id}</remarks>
    /// <returns>A StatusCode and a CommentDTO</returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ValidatedResult<CommentDTO>>> Update(CreateCommentDTO update_comment_dto, int id)
    {
        try
        {
            ValidationResult validation = await _validator.ValidateAsync(update_comment_dto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            Comment comment = _mapper.Map<Comment>(update_comment_dto);
            ValidatedResult<Comment> result = await _commentService.UpdateAsync(comment, id);
            if (result.Success is false || result.Value is null) { return ValidatedResult<CommentDTO>.Failed(0, "This Comment is not registered"); }

            CommentDTO? comment_dto = _mapper.Map<CommentDTO>(result.Value);

            return StatusCode(200, ValidatedResult<CommentDTO>.Passed(comment_dto));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidatedResult<CommentDTO>.Failed(0, ex.Message));
        }
    }

    /// <summary>
    /// Delete a Comment
    /// </summary>
    /// <param name="id"></param>
    /// <remarks>DELETE https://localhost:7022/api/comment/{id}</remarks>
    /// <returns>A boolean</returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ValidatedResult<bool>>> Delete(int id)
    {
        try
        {
            ValidatedResult<bool> result = await _commentService.DeleteAsync(id);
            if (result.Success is false || result.Value is false) { return ValidatedResult<bool>.Failed(0, "This comment is not registered"); }

            return StatusCode(200, ValidatedResult<bool>.Passed(result.Value));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidatedResult<bool>.Failed(0, ex.Message));
        }
    }
}
