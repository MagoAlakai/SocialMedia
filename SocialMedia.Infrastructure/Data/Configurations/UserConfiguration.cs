namespace SocialMedia.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
    builder.ToTable("Users");

    builder.HasKey(e => e.Id);

    builder.Property(e => e.Name)
        .IsRequired()
        .HasMaxLength(50)
        .IsUnicode(false);

    builder.Property(e => e.LastName)
        .IsRequired()
        .HasMaxLength(100)
        .IsUnicode(false);

    builder.Property(e => e.BirthDate)
        .IsRequired()
        .HasColumnType("datetime");

    builder.Property(e => e.PhoneNumber)
        .IsRequired()
        .HasMaxLength(25)
        .IsUnicode(false);

    builder.HasMany(e => e.Posts)
        .WithOne(x => x.User)
        .OnDelete(DeleteBehavior.NoAction)
        .HasConstraintName("FK_User_Posts");

    builder.HasMany(e => e.Comments)
        .WithOne(x => x.User)
        .OnDelete(DeleteBehavior.NoAction)
        .HasConstraintName("FK_User_Comments");
    }
}
