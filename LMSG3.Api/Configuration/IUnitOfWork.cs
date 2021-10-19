using LMSG3.Api.Services.Repositories;
using System.Threading.Tasks;

namespace LMSG3.Api.Configuration
{
    public interface IUnitOfWork
    {
        ILiteratureRepository LiteratureRepository { get; }
        ILiteratureAuthorRepository LiteratureAuthorRepository { get; }
        Task CompleteAsync();
        void Dispose();
    }
}