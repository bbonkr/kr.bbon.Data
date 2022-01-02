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
        }
    }

}
