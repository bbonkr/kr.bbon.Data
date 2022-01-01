using kr.bbon.Data.Abstractions.Entities;

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
            if (typeof(IEntityHasIdentifier<>).IsAssignableFrom(EntityType))
            {
                // Hack
                var identifierColumnName = nameof(IEntityHasIdentifier<string>.Id);
                builder.HasKey(identifierColumnName);

                var identifierPropertyBuilder = builder.Property(identifierColumnName)
                    .IsRequired();

                if (EntityType.GenericTypeArguments.Length > 1)
                {
                    var keyType = EntityType.GenericTypeArguments[1];

                    identifierPropertyBuilder.HasConversion(keyType);
                    identifierPropertyBuilder.ValueGeneratedOnAdd();
                }
            }

            if (typeof(IEntitySupportSoftDeletion).IsAssignableFrom(EntityType))
            {
                builder.Property(nameof(IEntitySupportSoftDeletion.IsDeleted))
                    .IsRequired(databaseOptions.UseSoftDelete)
                    .HasDefaultValue(false)
                    .HasValueGenerator<IsDeletedValueGenerator>();

                builder.Property(nameof(IEntitySupportSoftDeletion.DeletedAt))
                    .IsRequired(false)
                    .HasValueGenerator<DateTimeOffsetValueGenerator>();


                if (databaseOptions.UseSoftDelete)
                {
                    builder.HasQueryFilter(x => (x as IEntitySupportSoftDeletion).IsDeleted != true);
                }
            }

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasValueGenerator<DateTimeOffsetValueGenerator>();

            builder.Property(x => x.UpdatedAt)
                .IsRequired(false)
                .HasValueGenerator<DateTimeOffsetValueGenerator>();

            ConfigureEntity(builder);
        }

        public abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);

        protected readonly DatabaseOptions databaseOptions;

        public Type EntityType => typeof(TEntity);
    }
}
