using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace kr.bbon.Data.Abstractions
{
    public interface IDataService
    {
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Serializable);

        Task ComminAsync(CancellationToken cancellationToken = default);

        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
