using kr.bbon.Data.Abstractions.Entities;

namespace Example.Entities;

public class User : EntitySupportSoftDeletionBase<long>
{
    public string UserName { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; }

    public virtual ICollection<Post> Posts { get; set; }

    //public override bool IsKeyValueGeneratedOnAdd() => true;
}
