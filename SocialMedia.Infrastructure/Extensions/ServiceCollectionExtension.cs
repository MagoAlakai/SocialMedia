namespace SocialMedia.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplicationDbContext(this IServiceCollection services, IConfiguration configurations)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configurations.GetConnectionString(nameof(ApplicationDbContext)),
            x => x.MigrationsAssembly("SocialMedia.Infrastructure")));
    }
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
    }
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreatePostDTO>, PostValidator>();
        services.AddScoped<IValidator<CreateUserDTO>, UserValidator>();
        services.AddScoped<IValidator<CreateCommentDTO>, CommentValidator>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IPostService, PostService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ICommentService, CommentService>();
        services.AddTransient<IIdentityService, IdentityService>();
    }
}
