namespace SocialMedia.UnitTest.Posts;

[TestClass]
public class PostControllerUnitTests
{
    private readonly JsonSerializerOptions? _json_serializer_options;
    private readonly Mock<IUnitOfWork> _unit;
    private readonly Fixture _fixture;
    private PostService? _postService;

    public PostControllerUnitTests()
    {
        _json_serializer_options = new()
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        _unit = new Mock<IUnitOfWork>();
        _fixture = new Fixture();
    }

    private void ConsoleWriteObject(string prefix, object? obj)
    {
        string str_json = JsonSerializer.Serialize(obj, _json_serializer_options);
        Console.WriteLine($"{prefix}: {str_json}");
        _ = str_json;
        _ = prefix;
    }

    [TestMethod]
    public async Task GetPostsToSuccess()
    {
        //Arrange
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        List<Post> posts = _fixture.CreateMany<Post>(3).ToList();

        _unit.Setup(repo => repo.postRepository.GetAsync()).ReturnsAsync(posts);
        _postService = new PostService(_unit.Object);

        //Act
        ValidatedResult<IEnumerable<Post>> result = await _postService.GetAsync();
        ConsoleWriteObject("Result", result);

        //Assert
        Assert.IsNotNull(result.Value);
        Assert.AreEqual(true, result.Success);
    }

    [TestMethod]
    public async Task GetPostsToFailure()
    {
        //Arrange
        _unit.Setup(repo => repo.postRepository.GetAsync());
        _postService = new PostService(_unit.Object);

        //Act
        ValidatedResult<IEnumerable<Post>>? result = await _postService.GetAsync();
        ConsoleWriteObject("Result", result);

        //Assert
        Assert.IsNull(result.Value.Value);
        Assert.AreEqual(result.Value.FailureMessage, "There are no Posts registered");
        Assert.AreEqual(false, result.Value.Success);
    }
}
