using LMSG3.Core.Repositories;
using System.Threading.Tasks;

namespace LMSG3.Core.Configuration
{
    public interface IUnitOfWork
    {
        ILiteratureRepository LiteratureRepository { get; }
        Task CompleteAsync();
        void Dispose();
    }
}