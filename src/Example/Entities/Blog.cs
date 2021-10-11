using kr.bbon.Data;
using System.Collections.Generic;

namespace Example.Entities
{
    public class Blog : Entity
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public long OwnerId { get; set; }

        public virtual User Owner { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}