using Example.Entities;

using kr.bbon.Data;

using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Example.Data.EntityTypeConfigurations
{
    public class BlogTypeConfiguration : EntityTypeConfigurationBase<Blog>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Blog> builder)
        {

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            //builder.HasOne(x => x.Owner)
            //    .WithMany(x => x.Blogs)
            //    .HasForeignKey(x => x.OwnerId);

            builder.HasMany(x => x.Posts)
                .WithOne(x => x.Blog)
                .HasForeignKey(x => x.BlogId);
        }
    }
}
