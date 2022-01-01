using Example.Entities;
using kr.bbon.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace EntityTypeConfigurations
{
    public partial class BlogTypeConfiguration
    {
        public class UserTypeConfiguration : EntityTypeConfiguration<User>
        {
            public UserTypeConfiguration(IOptionsMonitor<DatabaseOptions> databaseOptionsAccessor) : base(databaseOptionsAccessor)
            {
            }

            public override void ConfigureEntity(EntityTypeBuilder<User> builder)
            {
                //builder.HasKey(x => x.Id);

                //builder.Property(x => x.Id)
                //    .IsRequired()
                //    .ValueGeneratedOnAdd();

                builder.Property(x => x.UserName)
                    .IsRequired();
            }
        }
    }
}
