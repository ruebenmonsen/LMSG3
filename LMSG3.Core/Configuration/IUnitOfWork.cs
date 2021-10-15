using LMSG3.Core.Models.Entities;
using LMSG3.Core.Repositories;
using System.Threading.Tasks;

namespace LMSG3.Core.Configuration
{
    public interface IUnitOfWork
    {
        ILiteratureRepository LiteratureRepository { get; }
        ILiteratureAuthorRepository LiteratureAuthorRepository { get; }
        
        ICourseRepository CourseRepository { get; }
        IRepository<Module> ModuleRepository { get; }
        IRepository<Activity> ActivityRepository {  get; }
        
        Task CompleteAsync();
        void Dispose();
        Task<bool> CompleteAsyncCheck();
    }
}