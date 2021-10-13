using LMSG3.Core.Models.Dtos;
using LMSG3.Core.Models.Entities;
using LMSG3.Core.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Data.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly ApplicationDbContext db;
        public UserRepository(ApplicationDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync(UserManager<ApplicationUser> userManager)
        {
            var usersDto = new List<UserDto>();
            var users = userManager.Users.OrderBy(u=>u.FName);

            foreach (var user in users )
            {
                string str = "";
                var role = await userManager.GetRolesAsync(user);
                
                    str = (str == "") ? role[0].ToString() : str + " - " + role[0].ToString();

                var userDto = new UserDto()
                {
                    FullName = user.FName + " " + user.LName,
                    Email = user.Email,
                    Role = str
                };
                usersDto.Add(userDto);
            }
        
            return usersDto;

        }
    }
}
