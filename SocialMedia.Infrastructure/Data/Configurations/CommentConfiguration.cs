namespace SocialMedia.Infrastructure.Data.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
    builder.ToTable("Comments");

    builder.HasKey(e => e.Id);

    builder.Property(e => e.PostId);

    builder.Property(e => e.UserId);

    builder.Property(e => e.Description)
        .IsRequired()
        .HasMaxLength(200)
        .IsUnicode(false);

    builder.Property(e => e.Date)
        .IsRequired()
        .HasColumnType("datetime");

    builder.HasOne(e => e.Post)
        .WithMany(x => x.Comments)
        .HasForeignKey(e => e.PostId)
        .OnDelete(DeleteBehavior.Cascade)
        .HasConstraintName("FK_Comment_Post");
    }
}
