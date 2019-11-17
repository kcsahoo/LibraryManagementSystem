using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories;
using DAL.Repositories.Interfaces;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly ApplicationDbContext _context;

        IRegisteredUserRepository _registeredusers;
        IBookRepository _books;
        IReservationsRepository _reservations;



        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        

        public IRegisteredUserRepository RegisteredUsers
        {
            get
            {
                if (_registeredusers == null)
                    _registeredusers = new RegisteredUserRepository(_context);

                return _registeredusers;
            }
        }



        public IBookRepository Books
        {
            get
            {
                if (_books == null)
                    _books = new BookRepository(_context);

                return _books;
            }
        }



        public IReservationsRepository Reservations
        {
            get
            {
                if (_reservations == null)
                    _reservations = new ReservationsRepository(_context);

                return _reservations;
            }
        }


        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
