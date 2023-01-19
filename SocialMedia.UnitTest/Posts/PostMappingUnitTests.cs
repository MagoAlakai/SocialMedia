namespace SocialMedia.UnitTest.Posts;

[TestClass]
public class PostMappingUnitTests
{
    private readonly JsonSerializerOptions? _json_serializer_options;
    private readonly Fixture _fixture;

    public PostMappingUnitTests()
    {
        _json_serializer_options = new()
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
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
    public void MapPostToPostDTOToSuccess()
    {
        //Arrange
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        Post post = _fixture.Create<Post>();

        MapperConfiguration config = new(cfg => cfg.AddProfile<AutoMapperProfiles>());
        IMapper mapper = config.CreateMapper();

        //Act
        PostDTO post_dto = mapper.Map<PostDTO>(post);

        ConsoleWriteObject("Post", post);
        ConsoleWriteObject("PostDTO", post_dto);

        //Assert
        Assert.AreEqual(post.Id, post_dto.Id);
        Assert.AreEqual(post.Image, post_dto.Image);
        Assert.AreEqual(post.Date, post_dto.Date);
        Assert.AreEqual(post.UserId, post_dto.UserId);
    }

    [TestMethod]
    public void MapPostToPostDTOToFailure()
    {
        //Arrange
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        Post post = _fixture.Create<Post>();
        post.Image = null;

        MapperConfiguration config = new(cfg => cfg.AddProfile<AutoMapperProfiles>());
        IMapper mapper = config.CreateMapper();

        //Act
        PostDTO post_dto = mapper.Map<PostDTO>(post);

        ConsoleWriteObject("Post", post);
        ConsoleWriteObject("PostDTO", post_dto);

        //Assert
        Assert.IsTrue(string.IsNullOrEmpty(post.Image));
        Assert.IsTrue(string.IsNullOrEmpty(post_dto.Image));

    }
}
