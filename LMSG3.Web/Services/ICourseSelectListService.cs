using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSG3.Web.Services
{
    public interface ICourseSelectListService
    {
        Task<IEnumerable<SelectListItem>> GetCourseAsync();
    }
}