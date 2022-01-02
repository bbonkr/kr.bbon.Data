using System;

using kr.bbon.Data.Abstractions.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace kr.bbon.Data
{
    public interface IHasEntityType
    {
        Type EntityType { get; }
    }

    public abstract class EntityTypeConfigurationBase<TEntity> : IHasEntityType, IEntityTypeConfiguration<TEntity> where TEntity : class, IEntity
    {
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
                    .IsRequired(true)
                    .HasDefaultValue(false)
                    .HasValueGenerator<IsDeletedValueGenerator>();

                builder.Property(nameof(IEntitySupportSoftDeletion.DeletedAt))
                    .IsRequired(false)
                    .HasValueGenerator<DateTimeOffsetValueGenerator>();

                builder.HasQueryFilter(x => (x as IEntitySupportSoftDeletion).IsDeleted != true);

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

        public Type EntityType => typeof(TEntity);
    }
}
