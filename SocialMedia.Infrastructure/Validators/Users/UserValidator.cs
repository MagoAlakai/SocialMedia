namespace SocialMedia.Infrastructure.Validators.Users;

public class UserValidator : AbstractValidator<CreateUserDTO>
{
    public UserValidator()
    {
        RuleFor(entity => entity.Name)
            .NotEmpty()
            .WithMessage("Name must not be empty")
            .NotNull()
            .WithMessage("Name must not be null")
            .WithSeverity(Severity.Warning);

        RuleFor(entity => entity.LastName)
            .NotEmpty()
            .WithMessage("LastName must not be empty")
            .NotNull()
            .WithMessage("LastName must not be null")
            .WithSeverity(Severity.Warning);

        RuleFor(entity => entity.Email)
            .EmailAddress()
            .WithMessage("This is not a valid Email format")
            .NotEmpty()
            .WithMessage("Email must not be empty")
            .NotNull()
            .WithMessage("Email must not be null")
            .WithSeverity(Severity.Warning);

        RuleFor(entity => entity.BirthDate)
            .NotEmpty()
            .NotNull()
            .WithSeverity(Severity.Warning);

        RuleFor(entity => entity.PhoneNumber)
            .NotEmpty()
            .WithMessage("PhoneNumber must not be empty")
            .NotNull()
            .WithMessage("PhoneNumber must not be null")
            .WithSeverity(Severity.Warning);

        RuleFor(entity => entity.Active)
            .NotEmpty()
            .WithMessage("Active must not be empty")
            .NotNull()
            .WithMessage("Active must not be null")
            .WithSeverity(Severity.Warning);
    }
}
