namespace SocialMedia.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {

    }
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        Posts = Set<Post>();
        Comments = Set<Comment>();
        Users = Set<User>();
    }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableSensitiveDataLogging();

        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetDirectoryRoot(@"C:\"))
               .AddJsonFile("Users\\magoa\\source\\personal\\SocialMedia\\SocialMedia.Api\\appsettings.json")
               .Build();
            var connectionString = configuration.GetConnectionString(nameof(ApplicationDbContext));
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CommentConfiguration());

        modelBuilder.ApplyConfiguration(new PostConfiguration());

        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
