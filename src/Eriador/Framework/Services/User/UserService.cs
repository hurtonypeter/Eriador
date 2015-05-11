using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eriador.Models.Data;
using Eriador.Models.Data.Entity;
using Microsoft.AspNet.Mvc;

namespace Eriador.Framework.Services.UserService
{
    public class UserService : IUserService
    {
        
        private readonly ApplicationDbContext db;

        public UserService(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task<List<Eriador.Models.Data.Entity.User>> GetAllUser()
        {
            return await db.Users.Where(u => u.UserName != "anonymous").ToListAsync();
        }

        public async Task<Eriador.Models.Data.Entity.User> GetUserById(int id)
        {
            return await db.Users.Where(u => u.UserName != "anonymous").SingleOrDefaultAsync(u => u.Id == id);
        }
    }
}
