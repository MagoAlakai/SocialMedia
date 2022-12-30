namespace SocialMedia.Core.Exceptions;

public class BusinessException : Exception
{
    public BusinessException(Exception ex)
    {
    }
    public BusinessException(string? message) : base(message)
    {
    }
}
