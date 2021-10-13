using LMSG3.Core.Models.Dtos;
using LMSG3.Core.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Core.Repositories
{
    public interface IUserRepository
    {
        Task<UserDto> GetUsersAsync(UserManager<ApplicationUser> userManager);
    }
}
