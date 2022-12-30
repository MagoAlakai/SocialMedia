namespace SocialMedia.Api.Responses;

/// <summary>
/// Create specific fotmat for responses
/// </summary>
/// <typeparam name="T"></typeparam>
public class ControllerResponse<T>
{
    public T Data { get; set; }
    public ControllerResponse(T data)
    {
        Data = data;
    }
}
