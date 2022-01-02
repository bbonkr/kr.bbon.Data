using kr.bbon.Data.Abstractions.Entities;

namespace Example.Entities
{

    public class Post : EntitySupportSoftDeletionBase<long>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public long BlogId { get; set; }

        public virtual Blog Blog { get; set; }

        public long AuthorId { get; set; }

        public virtual User Author { get; set; }
    }

}