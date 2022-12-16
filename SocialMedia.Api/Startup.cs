using Microsoft.EntityFrameworkCore;
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

        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(_configurations.GetConnectionString(nameof(ApplicationDbContext)),
            x => x.MigrationsAssembly("SocialMedia.Infrastructure")));

        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        //Add Repositories
        services.AddTransient<IPostRepository, PostRepository>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SocialMedia v1"));
        }

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        if (app is null)
        {
            throw new Exception($"IApplicationBuilder ({nameof(app)}) arrived as null at Startup.Configure()");
        }

        app.UseRouting();
        app.UseCors();
        app.UseAuthorization();

        app.UseResponseCaching();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}




