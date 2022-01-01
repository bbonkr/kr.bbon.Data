using System;

namespace kr.bbon.Data.Abstractions.Entities
{
    /// <summary>
    /// Base class of Entity classes.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Represent when entry has been created.
        /// </summary>
        DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Represent when entry has been updated latest.
        /// </summary>
        DateTimeOffset? UpdatedAt { get; set; }
    }
}
