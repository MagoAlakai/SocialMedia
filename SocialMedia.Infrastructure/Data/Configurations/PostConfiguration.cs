namespace SocialMedia.Infrastructure.Data.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id);

        builder.Property(e => e.Image);

        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(200)
            .IsUnicode(false);

        builder.Property(e => e.Date)
            .IsRequired()
            .HasColumnType("datetime");

        builder.HasOne(e => e.User)
            .WithMany(x => x.Posts)
            .HasForeignKey(e => e.UserId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Post_User");
    }
}
