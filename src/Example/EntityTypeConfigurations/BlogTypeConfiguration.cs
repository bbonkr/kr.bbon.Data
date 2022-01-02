using Example.Entities;

using kr.bbon.Data;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace Example.EntityTypeConfigurations
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

            builder.HasOne(x => x.Owner)
                .WithMany(x => x.Blogs)
                .HasForeignKey(x => x.OwnerId);
        }
    }
}
