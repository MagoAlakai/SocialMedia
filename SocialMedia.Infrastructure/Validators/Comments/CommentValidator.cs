namespace SocialMedia.Infrastructure.Validators.Comments;

public class CommentValidator : AbstractValidator<CreateCommentDTO>
{
    public CommentValidator()
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

        RuleFor(entity => entity.PostId)
            .NotEmpty()
            .WithMessage("PostId must not be empty")
            .NotNull()
            .WithMessage("PostId must not be null")
            .WithSeverity(Severity.Warning);

        RuleFor(entity => entity.Date)
            .NotEmpty()
            .NotNull()
            .WithSeverity(Severity.Warning);

        RuleFor(entity => entity.Active)
            .NotEmpty()
            .WithMessage("Active must not be empty")
            .NotNull()
            .WithMessage("Active must not be null")
            .WithSeverity(Severity.Warning);
    }
}
