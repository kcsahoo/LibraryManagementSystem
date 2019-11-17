using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        IEnumerable<Book> GetAllBooks();
        Book GetBookByID(int ID);
        Book GetBookByName(string name);
        Book AddBook(Book book);
        Book UpdateBook(Book book);
        int DeleteBook(int ID);

    }
}
