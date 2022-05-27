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
            //if (typeof(IEntityHasIdentifier<>).IsAssignableFrom(EntityType))
            if (IsAssignableToGenericType(EntityType, typeof(IEntityHasIdentifier<>)))
            {
                // Hack
                var identifierColumnName = nameof(IEntityHasIdentifier<long>.Id);
                builder.HasKey(identifierColumnName);

                var identifierPropertyBuilder = builder.Property(identifierColumnName)
                    .IsRequired();

                if (EntityType.GenericTypeArguments.Length > 1 || (EntityType.BaseType != null && EntityType.BaseType.GenericTypeArguments.Length > 1))
                {
                    // has key conversion type
                    var keyConversionType = EntityType.GenericTypeArguments.Length > 1 ?
                        EntityType.GenericTypeArguments[1] :
                        (EntityType.BaseType != null && EntityType.BaseType.GenericTypeArguments.Length > 1) ?
                        EntityType.BaseType.GenericTypeArguments[1] :
                        null;

                    if (keyConversionType != null)
                    {
                        identifierPropertyBuilder.HasConversion(keyConversionType);
                    }
                }

                var defaultInstance = Activator.CreateInstance(EntityType);
                if (defaultInstance is IEntityHasIdentifier hasIdentifierEntity)
                {
                    if (hasIdentifierEntity.IsKeyValueGeneratedOnAdd())
                    {
                        // key value generated on add
                        identifierPropertyBuilder.ValueGeneratedOnAdd();
                    }
                }
            }

            if (typeof(IEntitySupportSoftDeletion).IsAssignableFrom(EntityType))
            {
                builder.Property(nameof(IEntitySupportSoftDeletion.IsDeleted))
                    .IsRequired(true)
                    .HasDefaultValue(false)
                    .HasConversion<bool>()
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

        private bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }

        public Type EntityType => typeof(TEntity);
    }
}
