

using kr.bbon.Data;

namespace Example.Entities
{

    public class Post : Entity
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public long BlogId { get; set; }

        public virtual Blog Blog { get; set; }

        public long AuthorId { get; set; }

        public virtual User Author { get; set; }
    }

}