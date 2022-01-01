using Example.Entities;

using kr.bbon.Data.Abstractions;

namespace Example.HostedServices
{
    public class GetUserListSpecification : SpecificationBase<User>
    {
        public GetUserListSpecification()
        {
            AddInclude(x => x.Blogs);
            AddInclude(x => x.Posts);
            AddSort(x => x.UserName, SortOrder.Ascending);
        }
    }
}
