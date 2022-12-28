namespace SocialMedia.Api.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;
    private readonly IValidator<CreateCommentDTO> _validator;

    public CommentController(ICommentService commentService, IValidator<CreateCommentDTO> validator)
    {
        _commentService = commentService;
        _validator = validator;
    }

    //GET https://localhost:7022/api/comment
    [HttpGet]
    public async Task<IActionResult> GetComments()
    {
        try
        {
            IEnumerable<CommentDTO> result = await _commentService.GetAsync();
            if (result is null) { return BadRequest($"There are no comments registered yet"); }

            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex.Message));
        }
    }

    //GET https://localhost:7022/api/comment/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            CommentWithUserAndPostDTO result = await _commentService.GetByIdAsync(id);
            if (result is null) { return BadRequest($"This comment is not registered"); }

            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex.Message));
        }
    }

    //POST https://localhost:7022/api/comment/
    [HttpPost]
    public async Task<IActionResult> PostPost(CreateCommentDTO create_comment_dto)
    {
        try
        {
            ValidationResult validation = await _validator.ValidateAsync(create_comment_dto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            CommentDTO? result = await _commentService.PostAsync(create_comment_dto);
            if (result is null) { return BadRequest($"This comment is already registered"); }

            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex.Message));
        }
    }

    //PUT https://localhost:7022/api/comment/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(CreateCommentDTO update_comment_dto, int id)
    {
        try
        {
            ValidationResult validation = await _validator.ValidateAsync(update_comment_dto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            CommentDTO? result = await _commentService.UpdateAsync(update_comment_dto, id);
            if (result is null) { return BadRequest($"This post is not registered"); }

            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex.Message));
        }
    }

    //DELETE https://localhost:7022/api/comment/{id}
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
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
