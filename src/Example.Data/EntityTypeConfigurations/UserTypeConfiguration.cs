using Example.Entities;

using kr.bbon.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Example.Data.EntityTypeConfigurations
{

    public class UserTypeConfiguration : EntityTypeConfigurationBase<User>
    {
        public override void ConfigureEntity(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.UserName)
                .IsRequired();

            builder.HasMany(x => x.Blogs)
                .WithOne(x => x.Owner)
                .HasForeignKey(x => x.OwnerId);

            builder.HasMany(x => x.Posts)
                .WithOne(x => x.Author)
                .HasForeignKey(x => x.AuthorId);
        }
    }

}
