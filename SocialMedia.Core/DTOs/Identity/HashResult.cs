namespace SocialMedia.Core.DTOs.Identity;

public class HashResult
{
    public string? Hash { get; set; }
    public byte[]? Salt { get; set; }
}
