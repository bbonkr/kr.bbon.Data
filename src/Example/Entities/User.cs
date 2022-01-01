using kr.bbon.Data.Abstractions.Entities;

using System.Collections.Generic;

namespace Example.Entities
{
    public class User : EntitySupportSoftDeletionBase<long>
    {
        //public long Id { get; set; }

        public string UserName { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }

}