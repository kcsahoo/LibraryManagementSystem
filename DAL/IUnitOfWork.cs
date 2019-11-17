using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IUnitOfWork
    {
        IRegisteredUserRepository RegisteredUsers { get; }
        IBookRepository Books { get; }
        IReservationsRepository Reservations { get; }


        int SaveChanges();
    }
}
