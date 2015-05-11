using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eriador.Models.Data.Entity;

namespace Eriador.Framework.Services.UserService
{
    public interface IUserService
    {
        Task<List<Eriador.Models.Data.Entity.User>> GetAllUser();

        Task<Eriador.Models.Data.Entity.User> GetUserById(int id);
    }
}
