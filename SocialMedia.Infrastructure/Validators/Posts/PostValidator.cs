namespace SocialMedia.Infrastructure.Validators.Posts;

public class PostValidator : AbstractValidator<CreatePostDTO>
{
    public PostValidator()
    {
        RuleFor(entity => entity.Description)
            .NotEmpty()
            .WithMessage("Description must not be empty")
            .NotNull()
            .WithMessage("Description must not be null")
            .WithSeverity(Severity.Warning);

        RuleFor(entity => entity.UserId)
            .NotEmpty()
            .WithMessage("UserId must not be empty")
            .NotNull()
            .WithMessage("UserId must not be null")
            .WithSeverity(Severity.Warning);

        RuleFor(entity => entity.Date)
            .NotEmpty()
            .NotNull()
            .WithSeverity(Severity.Warning);

        RuleFor(entity => entity.Image)
            .NotEmpty()
            .WithMessage("Image must not be empty")
            .NotNull()
            .WithMessage("Image must not be null")
            .Must(BeValidUrl)
            .WithMessage("This is not a valid URL")
            .WithSeverity(Severity.Warning);
    }
    private bool BeValidUrl(string? value) => Uri.TryCreate(value, UriKind.Absolute, out _);
}
