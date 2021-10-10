using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace kr.bbon.Data
{
    public class IsDeletedValueGenerator : ValueGenerator
    {
        public override bool GeneratesTemporaryValues => false;

        protected override object NextValue( EntityEntry entry)
        {
            return false;
        }
    }
}
