using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class RegisteredUserRepository : Repository<RegisteredUser>, IRegisteredUserRepository
    {
        public RegisteredUserRepository(ApplicationDbContext context) : base(context)
        { }



        public IEnumerable<RegisteredUser> GetAllRegisteredUsers()
        {
            return _appContext.RegisteredUsers
                .OrderBy(c => c.Name)
                .ToList();
        }

        public RegisteredUser GetRegisteredUserByID(int ID)
        {
            return _appContext.RegisteredUsers
            .Where(b => b.Id == ID)
            .FirstOrDefault();

        }
        public RegisteredUser GetRegisteredUserByName(string name)
        {
            return _appContext.RegisteredUsers
            .Where(b => b.Name.ToLower().Contains(name.ToLower()))
            .FirstOrDefault();
        }

        public RegisteredUser AddRegisteredUser(RegisteredUser registeredUser)
        {
            RegisteredUser existingRegisteredUser = _appContext.RegisteredUsers
                .Where(b => b.Name.ToLower().Trim()==registeredUser.Name.ToLower().Trim())
                .FirstOrDefault();
            if (existingRegisteredUser != null)
            {
                return null;
            }
            else
            {
                _appContext.Add<RegisteredUser>(registeredUser);
                return registeredUser;
            }
        }

        public RegisteredUser UpdateRegisteredUser(RegisteredUser registeredUser)
        {
            RegisteredUser existingRegisteredUser = GetRegisteredUserByID(registeredUser.Id);
            if (existingRegisteredUser != null)
            {
                existingRegisteredUser.Name = registeredUser.Name;
                existingRegisteredUser.Email = registeredUser.Email;
                existingRegisteredUser.PhoneNumber = registeredUser.PhoneNumber;
                existingRegisteredUser.Address = registeredUser.Address;
                existingRegisteredUser.City = registeredUser.City;
                existingRegisteredUser.Gender = registeredUser.Gender;
                existingRegisteredUser.UpdatedBy = registeredUser.UpdatedBy;
                existingRegisteredUser.UpdatedDate = DateTime.UtcNow;
                _appContext.Update(existingRegisteredUser);
                return existingRegisteredUser;
            }
            else
            {
                return null;
            }
        }

        public int DeleteRegisteredUser(int ID)
        {
            RegisteredUser registeredUser = GetRegisteredUserByID(ID);
            if (registeredUser != null)
            {
                _appContext.Remove(registeredUser);
                return ID;
            }
            else
            {
                return 0;
            }

        }

        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
    }
}
