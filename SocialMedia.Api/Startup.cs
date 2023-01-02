namespace SocialMedia.Api;

public class Startup
{
    private readonly IConfiguration _configurations;
    public Startup(IConfiguration configurations) => _configurations = configurations;

    // This method gets called by the runtime. Use this method to configure the services.
    public void ConfigureServices(IServiceCollection services)
    {
    // Add services to the container.

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddEndpointsApiExplorer();
        services.AddApplicationInsightsTelemetry(opt => opt.EnableActiveTelemetryConfigurationSetup = true);

        // Add from ServiceCollectionExtension
        services.AddApplicationDbContext(_configurations);
        services.AddIdentityConfig();
        services.AddControllersConfig();
        services.AddSwaggerGenConfig();
        services.AddServices();
        services.AddRepositories();
        services.AddValidators();

        //Configure Authentication with Bearer and JWT, and policies from ServiceCollectionExtension
        services.AddAuthenticationConfig(_configurations);
        services.AddAuthorizationConfig();
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




