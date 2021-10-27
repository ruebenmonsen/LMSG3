using LMSG3.Core.Models.Entities;
using LMSG3.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSG3.Core.Repositories
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> Getparticipants(int? id);
        int GetCourseId(string studentId);
    }
}