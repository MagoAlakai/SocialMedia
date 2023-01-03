namespace SocialMedia.Api.Controllers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    [AllowAnonymous]
    public async Task<ActionResult<ValidatedResult<IEnumerable<PostDTO>>>> GetPosts()
    {
        try
        {
            ValidatedResult<IEnumerable<Post>> result = await _postService.GetAsync();
            if (result.Success is false || result.Value is null) { return ValidatedResult<IEnumerable<PostDTO>>.Failed(0, $"There are no posts registered yet"); }

            List<PostDTO> post_dto_list = new();
            foreach (Post post in result.Value)
            {
                PostDTO post_dto = _mapper.Map<PostDTO>(post);
                post_dto_list.Add(post_dto);
            }

            return StatusCode(200, ValidatedResult<IEnumerable<PostDTO>>.Passed(post_dto_list));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidatedResult<IEnumerable<PostDTO>>.Failed(0,ex.Message));
        }
    }

    /// <summary>
    /// Get details fom specific Post, with User and Comments
    /// </summary>
    /// <param name="id">Id from the Post</param>
    /// <remarks>GET https://localhost:7022/api/post/{id}</remarks>
    /// <returns>A PostDTO</returns>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<ValidatedResult<PostWithUserAndCommentsDTO>>> GetById(int id)
    {
        try
        {
            ValidatedResult<Post> result = await _postService.GetByIdAsync(id);
            if (result.Success is false || result.Value is null) { return ValidatedResult< PostWithUserAndCommentsDTO>.Failed(0, $"This post is not registered"); }

            UserSimplifiedDTO user_dto = _mapper.Map<UserSimplifiedDTO>(result.Value.User);

            List<CommentSimplifiedDTO> comments_dto = _mapper.Map<List<CommentSimplifiedDTO>>(result.Value.Comments);

            PostWithUserAndCommentsDTO post_with_user_dto = _mapper.Map<Post, PostWithUserAndCommentsDTO>(result.Value, options =>
                   options.AfterMap((src, dest) =>
                   {
                       dest.User = user_dto;
                       dest.Comments = comments_dto;
                   }));

            return StatusCode(200, ValidatedResult<PostWithUserAndCommentsDTO>.Passed(post_with_user_dto));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidatedResult<PostWithUserAndCommentsDTO>.Failed(0, ex.Message));
        }
    }

    /// <summary>
    /// Create a new Post
    /// </summary>
    /// <param name="create_post_dto"></param>
    /// <remarks>POST https://localhost:7022/api/post/</remarks>
    /// <returns>A StatusCode and the PostDTO</returns>
    [HttpPost]
    public async Task<ActionResult<ValidatedResult<PostDTO>>> Post(CreatePostDTO create_post_dto)
    {
        try
        {
            ValidationResult validation = await _validator.ValidateAsync(create_post_dto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            Post post = _mapper.Map<Post>(create_post_dto);
            ValidatedResult<Post> result = await _postService.PostAsync(post);
            if (result.Success is false || result.Value is null) { return ValidatedResult<PostDTO>.Failed(0, $"This post is already registered"); }

            PostDTO? post_dto = _mapper.Map<PostDTO>(result.Value);

            return StatusCode(200, ValidatedResult<PostDTO>.Passed(post_dto));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidatedResult<PostDTO>.Failed(0, ex.Message));
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
    public async Task<ActionResult<ValidatedResult<PostDTO>>> Update(CreatePostDTO update_post_dto, int id)
    {
        try
        {
            ValidationResult validation = await _validator.ValidateAsync(update_post_dto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            Post? post = _mapper.Map<Post>(update_post_dto);
            ValidatedResult<Post> result = await _postService.UpdateAsync(post, id);
            if (result.Success is false || result.Value is null) { return ValidatedResult<PostDTO>.Failed(0, $"This post is not registered"); }

            PostDTO? post_dto = _mapper.Map<PostDTO>(result.Value);

            return StatusCode(200, ValidatedResult<PostDTO>.Passed(post_dto));
        }
        catch (Exception ex)
        {
            return BadRequest(ValidatedResult<PostDTO>.Failed(0, ex.Message));
        }
    }

    /// <summary>
    /// Delete a Post
    /// </summary>
    /// <param name="id"></param>
    /// <remarks>DELETE https://localhost:7022/api/post/{id}</remarks>
    /// <returns>A boolean</returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ValidatedResult<bool>>> Delete(int id)
    {
        try
        {
            ValidatedResult<bool> result = await _postService.DeleteAsync(id);
            if (result.Success is false || result.Value is false) { return ValidatedResult<bool>.Failed(0, $"This post is not registered"); }

            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return BadRequest(ValidatedResult<bool>.Failed(0, ex.Message));
        }
    }
}
