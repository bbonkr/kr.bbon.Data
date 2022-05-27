using System;

namespace kr.bbon.Data.Abstractions.Entities
{
    public interface IEntityHasIdentifier
    {
        bool IsKeyValueGeneratedOnAdd();
    }

    public interface IEntityHasIdentifier<TKey> : IEntityHasIdentifier
    {
        TKey Id { get; set; }
    }

    public interface IEntityHasIdentifier<TKey, TKeyConversion> : IEntityHasIdentifier<TKey>
    {
        Type ConversionType { get; }
    }
}
