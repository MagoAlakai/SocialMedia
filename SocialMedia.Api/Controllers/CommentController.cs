using SocialMedia.Core.Data;
using System.Collections.Generic;

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
            if (result.Success is false) { return BadRequest($"There are no comments registered yet"); }

            List<CommentDTO> comment_dto_list = new(_mapper.Map<IEnumerable<CommentDTO>>(result));

            return StatusCode(200, ValidatedResult<IEnumerable<CommentDTO>>.Passed(comment_dto_list));
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex));
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
    public async Task<ActionResult<CommentWithUserAndPostDTO>> GetById(int id)
    {
        try
        {
            Comment? result = await _commentService.GetByIdAsync(id);
            if (result is null) { return BadRequest($"This comment is not registered"); }

            UserSimplifiedDTO user_dto = _mapper.Map<UserSimplifiedDTO>(result.User);
            PostSimplifiedDTO post_dto = _mapper.Map<PostSimplifiedDTO>(result.Post);

            CommentWithUserAndPostDTO comment_with_user_and_post_dto = _mapper.Map<Comment, CommentWithUserAndPostDTO>(result, options =>
                   options.AfterMap((src, dest) => {
                       dest.User = user_dto;
                       dest.Post = post_dto;
                   }));

            return StatusCode(200, comment_with_user_and_post_dto);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex));
        }
    }

    /// <summary>
    /// Create a Comment
    /// </summary>
    /// <param name="create_comment_dto"></param>
    /// <remarks>POST https://localhost:7022/api/comment/</remarks>
    /// <returns>A StatusCode and a CommentDTO</returns>
    [HttpPost]
    public async Task<ActionResult<CommentDTO>> Post(CreateCommentDTO create_comment_dto)
    {
        try
        {
            ValidationResult validation = await _validator.ValidateAsync(create_comment_dto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            Comment comment = _mapper.Map<Comment>(create_comment_dto);
            Comment? result = await _commentService.PostAsync(comment);
            if (result is null) { return BadRequest($"This comment is already registered"); }

            CommentDTO? comment_dto = _mapper.Map<CommentDTO>(result);

            return StatusCode(200, comment_dto);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex));
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
    public async Task<ActionResult<CommentDTO>> Update(CreateCommentDTO update_comment_dto, int id)
    {
        try
        {
            ValidationResult validation = await _validator.ValidateAsync(update_comment_dto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            Comment comment = _mapper.Map<Comment>(update_comment_dto);
            Comment? result = await _commentService.UpdateAsync(comment, id);
            if (result is null) { return BadRequest($"This post is not registered"); }

            CommentDTO? comment_dto = _mapper.Map<CommentDTO>(result);

            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex));
        }
    }

    /// <summary>
    /// Delete a Comment
    /// </summary>
    /// <param name="id"></param>
    /// <remarks>DELETE https://localhost:7022/api/comment/{id}</remarks>
    /// <returns>A boolean</returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        try
        {
            bool result = await _commentService.DeleteAsync(id);
            if (result is false) { return BadRequest($"This comment is not registered"); }

            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex.Message));
        }
    }
}
