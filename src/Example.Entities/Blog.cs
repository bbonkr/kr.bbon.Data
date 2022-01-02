using kr.bbon.Data.Abstractions.Entities;

using System.Collections.Generic;

namespace Example.Entities
{
    public class Blog : EntitySupportSoftDeletionBase<long>
    {
        public string Title { get; set; }

        public long OwnerId { get; set; }

        public virtual User Owner { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}