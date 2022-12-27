using SocialMedia.Core.Interfaces.Post;

namespace SocialMedia.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly IValidator<CreatePostDTO> _validator;

    public PostController(IPostService postService, IValidator<CreatePostDTO> validator)
    {
        _postService = postService;
        _validator = validator;
    }

    //GET https://localhost:7022/api/post
    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
        try
        {
            IEnumerable<PostDTO> result = await _postService.GetAsync();
            if (result is null) { return BadRequest($"There are no posts registered yet"); }

            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex.Message));
        }
    }

    //GET https://localhost:7022/api/post/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            PostDTO result = await _postService.GetByIdAsync(id);
            if (result is null) { return BadRequest($"This post is not registered"); }

            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex.Message));
        }
    }

    //POST https://localhost:7022/api/post/
    [HttpPost]
    public async Task<IActionResult> PostPost(CreatePostDTO create_post_dto)
    {
        try
        {
            ValidationResult validation = await _validator.ValidateAsync(create_post_dto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            PostDTO? result = await _postService.PostAsync(create_post_dto);
            if (result is null) { return BadRequest($"This post is already registered"); }

            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex.Message));
        }
    }

    //PUT https://localhost:7022/api/post/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(CreatePostDTO update_post_dto, int id)
    {
        try
        {
            ValidationResult validation = await _validator.ValidateAsync(update_post_dto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            PostDTO? result = await _postService.UpdateAsync(update_post_dto, id);
            if (result is null) { return BadRequest($"This post is not registered"); }

            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex.Message));
        }
    }

    //DELETE https://localhost:7022/api/post/{id}
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            bool result = await _postService.DeleteAsync(id);
            if (result is false) { return BadRequest($"This post is not registered"); }

            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex.Message));
        }
    }
}
