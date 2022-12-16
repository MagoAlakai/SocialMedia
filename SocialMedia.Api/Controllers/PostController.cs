namespace SocialMedia.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;

    public PostController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    //https://localhost:7022/api/post
    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
        IEnumerable<Post> posts = await _postRepository.GetPosts();
        return Ok(posts);
    }

    //https://localhost:7022/api/post
    [HttpPost]
    public IActionResult Post()
    {
        return Ok();
    }
}
