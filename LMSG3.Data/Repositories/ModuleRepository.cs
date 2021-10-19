using LMSG3.Core.Models.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Data.Repositories
{
    public class ModuleRepository : GenericRepository<Module>
    {
        public ModuleRepository(ApplicationDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}