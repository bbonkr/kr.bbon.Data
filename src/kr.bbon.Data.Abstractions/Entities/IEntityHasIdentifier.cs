using System;

namespace kr.bbon.Data.Abstractions.Entities
{
    public interface IEntityHasIdentifier<TKey>
    {
        TKey Id { get; set; }
    }
    public interface IEntityHasIdentifier<TKey, TKeyConversion> : IEntityHasIdentifier<TKey>
    {
        Type ConversionType { get; }
    }
}
