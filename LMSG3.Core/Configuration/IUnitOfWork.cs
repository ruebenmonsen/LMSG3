using System.Threading.Tasks;

namespace LMSG3.Core.Configuration
{
    public interface IUnitOfWork
    {
        //ILiteratureRepository LiteratureRepository { get; }
        //ILiteratureAuthorRepository LiteratureAuthorRepository { get; }
        Task CompleteAsync();
        void Dispose();
    }
}