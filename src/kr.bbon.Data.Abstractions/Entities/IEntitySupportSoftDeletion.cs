using System;

namespace kr.bbon.Data.Abstractions.Entities
{
    public interface IEntitySupportSoftDeletion
    {
        /// <summary>
        /// Represent whether entry was deleted.
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// If use soft-deletion feature, Represent when entry has been deleted
        /// </summary>
        DateTimeOffset? DeletedAt { get; set; }
    }
}
