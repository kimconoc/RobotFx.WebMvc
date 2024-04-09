using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotFx.Repositories.EntityFamework.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        int Commit(bool isTrackChanged = true);
        Task<int> CommitAsync(bool isTrackChanged = true);
        void RollBack();
    }
}
