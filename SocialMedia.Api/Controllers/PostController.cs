namespace SocialMedia.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly IValidator<CreatePostDTO> _validator;
    private readonly IMapper _mapper;

    public PostController(IPostService postService, IValidator<CreatePostDTO> validator, IMapper mapper)
    {
        _postService = postService;
        _validator = validator;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieve all the exisiting posts
    /// </summary>
    /// <remarks>GET https://localhost:7022/api/post</remarks>
    /// <returns>A list of PostDTO</returns>
    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
        try
        {
            IEnumerable<Post> result = await _postService.GetAsync();
            if (result is null) { return BadRequest($"There are no posts registered yet"); }

            List<PostDTO> post_dto_list = new(_mapper.Map<IEnumerable<PostDTO>>(result));

            return StatusCode(200, post_dto_list);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex));
        }
    }

    /// <summary>
    /// Get details fom specific Post, with User and Comments
    /// </summary>
    /// <param name="id">Id from the Post</param>
    /// <remarks>GET https://localhost:7022/api/post/{id}</remarks>
    /// <returns>A PostDTO</returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            Post? result = await _postService.GetByIdAsync(id);
            if (result is null) { return BadRequest($"This post is not registered"); }

            UserSimplifiedDTO user_dto = _mapper.Map<UserSimplifiedDTO>(result.User);

            List<CommentSimplifiedDTO> comments_dto = _mapper.Map<List<CommentSimplifiedDTO>>(result.Comments);

            PostWithUserAndCommentsDTO post_with_user_dto = _mapper.Map<Post, PostWithUserAndCommentsDTO>(result, options =>
                   options.AfterMap((src, dest) =>
                   {
                       dest.User = user_dto;
                       dest.Comments = comments_dto;
                   }));

            return StatusCode(200, post_with_user_dto);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex));
        }
    }

    /// <summary>
    /// Create a new Post
    /// </summary>
    /// <param name="create_post_dto"></param>
    /// <remarks>POST https://localhost:7022/api/post/</remarks>
    /// <returns>A StatusCode and the PostDTO</returns>
    [HttpPost]
    public async Task<IActionResult> Post(CreatePostDTO create_post_dto)
    {

        //ValidationResult validation = await _validator.ValidateAsync(create_post_dto);

        //if (!validation.IsValid)
        //{
        //    return BadRequest(validation.Errors);
        //}

        //PostDTO? result = await _postService.PostAsync(create_post_dto);
        //if (result is null) { return BadRequest($"This post is already registered"); }

        //return Ok(result);

        try
        {
            ValidationResult validation = await _validator.ValidateAsync(create_post_dto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            Post post = _mapper.Map<Post>(create_post_dto);
            Post? result = await _postService.PostAsync(post);
            if (result is null) { return BadRequest($"This post is already registered"); }

            PostDTO? post_dto = _mapper.Map<PostDTO>(result);

            return StatusCode(200, post_dto);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex));
        }
    }

    /// <summary>
    /// Update a Post
    /// </summary>
    /// <param name="update_post_dto"></param>
    /// <param name="id"></param>
    /// <remarks>PUT https://localhost:7022/api/post/{id}</remarks>
    /// <returns>A StatusCode and the PostDTO</returns>
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

            Post? post = _mapper.Map<Post>(update_post_dto);
            Post? result = await _postService.UpdateAsync(post, id);
            if (result is null) { return BadRequest($"This post is not registered"); }

            PostDTO? post_dto = _mapper.Map<PostDTO>(result);

            return StatusCode(200, post_dto);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(BadRequest(ex));
        }
    }

    /// <summary>
    /// Delete a Post
    /// </summary>
    /// <param name="id"></param>
    /// <remarks>DELETE https://localhost:7022/api/post/{id}</remarks>
    /// <returns>A boolean</returns>
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
            return await Task.FromResult(BadRequest(ex));
        }
    }
}
