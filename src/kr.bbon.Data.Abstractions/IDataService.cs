using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data;

namespace kr.bbon.Data.Abstractions
{
    public interface IDataService
    {
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Serializable);

        Task ComminAsync(CancellationToken cancellationToken = default);

        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
