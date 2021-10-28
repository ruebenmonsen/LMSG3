using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSG3.Web.Services
{
    public interface ILiteratureSelectService
    {
        Task<IEnumerable<SelectListItem>> GetLiteratureLeves();
       Task<IEnumerable<SelectListItem>> GetLiteraturesTypes();
        Task<IEnumerable<SelectListItem>> GetLiteraturesSubjects();
    }
}
