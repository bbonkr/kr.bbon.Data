using Example.Entities;

using kr.bbon.Data;

using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Example.Data.EntityTypeConfigurations
{
    public class PostTypeConfiguration : EntityTypeConfigurationBase<Post>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Post> builder)
        {
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.Content)
                .IsRequired();

            builder.Property(x => x.AuthorId)
                .IsRequired();

            builder.Property(x => x.BlogId)
                .IsRequired();

            //builder.HasOne(x => x.Author)
            //    .WithMany(x => x.Posts)
            //    .HasForeignKey(x => x.AuthorId);

            //builder.HasOne(x => x.Blog)
            //    .WithMany(x => x.Posts)
            //    .HasForeignKey(x => x.BlogId);
        }
    }

}
