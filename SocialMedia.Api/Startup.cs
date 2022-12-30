using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Api;

public class Startup
{
    private readonly IConfiguration _configurations;
    public Startup(IConfiguration configurations) => _configurations = configurations;

    // This method gets called by the runtime. Use this method to configure the services.
    public void ConfigureServices(IServiceCollection services)
    {
        // Add services to the container.

        // Add DbContext from ServiceCollectionExtension
        services.AddApplicationDbContext(_configurations);

        services.AddControllers(options => 
        {
            options.Filters.Add<GlobalExceptionFilter>();
        })
        .AddNewtonsoftJson(options =>
        { 
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen( options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "SocialMedia Api", Version = "v1" });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
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

        services.AddApplicationInsightsTelemetry(opt => opt.EnableActiveTelemetryConfigurationSetup = true);

        //Configure Authentication with Bearer and JWT, and policies from ServiceCollectionExtension
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                string? SecretKey = _configurations["Authentication:SecretKey"];
                if (string.IsNullOrEmpty(SecretKey)){ throw new ArgumentException($"{nameof(SecretKey)} must contain value."); };
                byte[] jwtkey_byte_array = Encoding.UTF8.GetBytes(SecretKey);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configurations["Authentication:Issuer"],
                    ValidAudience = _configurations["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(jwtkey_byte_array),
                    ClockSkew = TimeSpan.Zero
                };
            });
        services.AddAuthorization(options =>
        {
            options.AddPolicy("IsAdmin", policy => policy.RequireClaim("isAdmin"));
        });

        // Add from ServiceCollectionExtension
        services.AddServices();
        services.AddRepositories();
        services.AddValidators();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SocialMedia v1"));
       
        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        if (app is null)
        {
            throw new Exception($"IApplicationBuilder ({nameof(app)}) arrived as null at Startup.Configure()");
        }

        app.UseRouting();
        app.UseCors();
        app.UseAuthorization();
        app.UseAuthentication();

        app.UseResponseCaching();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}




