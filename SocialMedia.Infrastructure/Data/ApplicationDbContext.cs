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
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comments");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .ValueGeneratedNever();

            entity.Property(e => e.PostId);

            entity.Property(e => e.UserId);

            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.Property(e => e.Date)
                .IsRequired()
                .HasColumnType("datetime");

            entity.HasOne(e => e.Post)
                .WithMany(x => x.Comments)
                .HasForeignKey(e => e.PostId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Comment_Post");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("Posts");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id);

            entity.Property(e => e.Image);

            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.Property(e => e.Date)
                .IsRequired()
                .HasColumnType("datetime");

            entity.HasOne(e => e.User)
                .WithMany(x => x.Posts)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Post_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.BirthDate)
                .IsRequired()
                .HasColumnType("datetime");

            entity.Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.HasMany(e => e.Posts)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_User_Posts");

            entity.HasMany(e => e.Comments)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_User_Comments");
        });
    }
}
