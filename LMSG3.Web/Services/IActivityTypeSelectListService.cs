using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSG3.Web.Services
{
    public interface IActivityTypeSelectListService
    {
        Task<IEnumerable<SelectListItem>> GetActivityTypeAsync();
    }
}
