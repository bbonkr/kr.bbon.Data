using Example.Entities;
using kr.bbon.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.EntityTypeConfigurations
{
    public partial class BlogTypeConfiguration : EntityTypeConfiguration<Blog>
    {
        public BlogTypeConfiguration(IOptionsMonitor<DatabaseOptions> databaseOptionsAccessor) 
            : base(databaseOptionsAccessor)
        {
        }

        public override void ConfigureEntity(EntityTypeBuilder<Blog> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

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
