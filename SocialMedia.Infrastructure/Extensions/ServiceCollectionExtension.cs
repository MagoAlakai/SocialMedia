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

    public static void AddAuthenticationConfig(this IServiceCollection services, IConfiguration configurations)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                string? SecretKey = configurations["Authentication:SecretKey"];
                if (string.IsNullOrEmpty(SecretKey)) { throw new ArgumentException($"{nameof(SecretKey)} must contain value."); };
                byte[] jwtkey_byte_array = Encoding.UTF8.GetBytes(SecretKey);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configurations["Authentication:Issuer"],
                    ValidAudience = configurations["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(jwtkey_byte_array),
                    ClockSkew = TimeSpan.Zero
                };
            });
    }

    public static void AddAuthorizationConfig(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("IsAdmin", policy => policy.RequireClaim("isAdmin"));
        });
    }

    public static void AddIdentityConfig(this IServiceCollection services)
    {
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
    }

    public static void AddControllersConfig(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<GlobalExceptionFilter>();
        })
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });
    } 
    public static void AddSwaggerGenConfig(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "SocialMedia Api", Version = "v1" });
            var xmlFile = "SocialMedia.Api.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath, true);

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });
    }
}
