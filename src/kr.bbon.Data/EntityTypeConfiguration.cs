using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using System;

namespace kr.bbon.Data
{
    public interface IEntityType
    {
        Type EntityType { get; }
    }

    public abstract class EntityTypeConfiguration<TEntity> : IEntityType, IEntityTypeConfiguration<TEntity> where TEntity : class, IEntity
    {
        public EntityTypeConfiguration(IOptionsMonitor<DatabaseOptions> databaseOptionsAccessor)
        {
            databaseOptions = databaseOptionsAccessor.CurrentValue ?? new DatabaseOptions();
        }

        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(x => x.IsDeleted)
                .IsRequired(databaseOptions.UseSoftDelete)
                .HasValueGenerator<IsDeletedValueGenerator>();

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasValueGenerator<DateTimeOffsetValueGenerator>();

            builder.Property(x => x.UpdatedAt)
                .IsRequired(false)
                .HasValueGenerator<DateTimeOffsetValueGenerator>();

            builder.Property(x => x.DeletedAt)
                .IsRequired(false)
                .HasValueGenerator<DateTimeOffsetValueGenerator>();

            if (databaseOptions.UseSoftDelete)
            {
                builder.HasQueryFilter(x => !x.IsDeleted);
            }

            ConfigureEntity(builder);
        }

        public abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);

        protected readonly DatabaseOptions databaseOptions;

        public Type EntityType => typeof(TEntity);
    }
}
