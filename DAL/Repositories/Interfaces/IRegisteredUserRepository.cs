using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Interfaces
{
    public interface IRegisteredUserRepository : IRepository<RegisteredUser>
    {
        IEnumerable<RegisteredUser> GetAllRegisteredUsers();
        RegisteredUser GetRegisteredUserByID(int ID);
        RegisteredUser GetRegisteredUserByName(string name);
        RegisteredUser AddRegisteredUser(RegisteredUser book);
        RegisteredUser UpdateRegisteredUser(RegisteredUser book);
        int DeleteRegisteredUser(int ID);
    }
}
