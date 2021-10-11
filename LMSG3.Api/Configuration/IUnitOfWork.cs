using LMSG3.Api.Repositories;
using System.Threading.Tasks;

namespace LMSG3.Api.Configuration
{
    public interface IUnitOfWork
    {
        ILiteratureRepository LiteratureRepository { get; }
        Task CompleteAsync();
        void Dispose();
    }
}