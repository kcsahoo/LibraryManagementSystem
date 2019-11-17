using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(DbContext context) : base(context)
        { }

        public IEnumerable<Book> GetAllBooks()
        {
            return _appContext.Books
                .OrderBy(c => c.Name)
                .ToList();
        }
        public  Book GetBookByID(int ID)
        {
            return _appContext.Books
            .Where(b => b.Id == ID)
            .FirstOrDefault();

        }
        public Book GetBookByName(string name)
        {
            return _appContext.Books
            .Where(b => b.Name.ToLower().Contains(name.ToLower()))            
            .FirstOrDefault();
        }

        public Book AddBook(Book book)
        {
            Book existingbook = _appContext.Books
                .Where(b => b.Name.ToLower().Trim()== book.Name.ToLower().Trim())
                .FirstOrDefault();
            if (existingbook != null)
            {
                return null;
            }
            else
            {
                _appContext.Add<Book>(book);
                return book;
            }
        }

        public Book UpdateBook(Book book)
        {
            Book existingbook = GetBookByID(book.Id);
            if (existingbook != null)
            {
                existingbook.Name = book.Name;
                existingbook.Description = book.Description;
                existingbook.UnitsInStock = book.UnitsInStock;
                existingbook.TotalUnits = book.TotalUnits;
                existingbook.UpdatedBy = book.UpdatedBy;
                existingbook.UpdatedDate = DateTime.UtcNow;
                _appContext.Update(existingbook);
                return book;
            }
            else
            {
                return null;
            }
        }

        public int DeleteBook(int ID)
        {
            Book book = GetBookByID(ID);
            if (book != null)
            {
                _appContext.Remove(book);
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
