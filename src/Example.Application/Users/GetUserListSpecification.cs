using Example.Entities;

using kr.bbon.Data.Abstractions.Specifications;

namespace Example.Application
{
    public class GetUserListSpecification : SpecificationBase<User>
    {
        public GetUserListSpecification()
        {
            AddInclude(x => x.Blogs);
            AddInclude(x => x.Posts);
            AddSort(nameof(User.UserName), true);
        }
    }
}
