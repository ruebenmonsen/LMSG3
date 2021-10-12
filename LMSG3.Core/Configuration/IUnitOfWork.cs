using LMSG3.Core.Repositories;
using System.Threading.Tasks;

namespace LMSG3.Core.Configuration
{
    public interface IUnitOfWork
    {
        ILiteratureRepository LiteratureRepository { get; }
        IUserRepository UserRepository { get; set; }
        Task CompleteAsync();
        void Dispose();
    }
}