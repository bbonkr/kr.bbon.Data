using Example.Entities;
using kr.bbon.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace Example.EntityTypeConfigurations
{
    public partial class BlogTypeConfiguration
    {
        public class PostTypeConfiguration : EntityTypeConfiguration<Post>
        {
            public PostTypeConfiguration(IOptionsMonitor<DatabaseOptions> databaseOptionsAccessor) : base(databaseOptionsAccessor)
            {
            }

            public override void ConfigureEntity(EntityTypeBuilder<Post> builder)
            {
                //builder.HasKey(x => x.Id);

                //builder.Property(x => x.Id)
                //    .IsRequired()
                //    .ValueGeneratedOnAdd();

                builder.Property(x => x.Title)
                    .IsRequired()
                    .HasMaxLength(1000);

                builder.Property(x => x.Content)
                    .IsRequired()
                    .HasField("Content");

                builder.Property(x => x.AuthorId)
                    .IsRequired();

                builder.Property(x => x.BlogId)
                    .IsRequired();

                builder.HasOne(x => x.Author)
                    .WithMany(x => x.Posts)
                    .HasForeignKey(x => x.AuthorId);

                builder.HasOne(x => x.Blog)
                    .WithMany(x => x.Posts)
                    .HasForeignKey(x => x.BlogId);
            }
        }
    }
}
